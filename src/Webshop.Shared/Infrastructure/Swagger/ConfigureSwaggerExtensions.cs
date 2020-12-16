using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Webshop.Shared.Infrastructure.Swagger
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureSwaggerExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration, string assemblyNameForXmlComments,
            string authorityKey = "Auth:Authority",
            string audienceKey = "Auth:Audience",
            string apiKeysKey = "Auth:ApiKeys:0")
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                foreach (var openApiInfo in options.SwaggerGeneratorOptions.SwaggerDocs.Values)
                {
                    openApiInfo.Title = assemblyNameForXmlComments;
                }
                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValuesFilter>();
                options.OperationFilter<FileUploadOperation>();

                options.CustomSchemaIds(schema => schema.FullName);
                options.CustomOperationIds(apiDesc => apiDesc.ActionDescriptor?.AttributeRouteInfo?.Name ??
                                                      (apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null));
                options.IncludeXmlComments(XmlCommentsFilePath(assemblyNameForXmlComments));

                if (!string.IsNullOrEmpty(configuration[apiKeysKey]))
                {
                    options.AddSecurityDefinition("apikey", new OpenApiSecurityScheme
                    {
                        Description = "API KEY Authorization header. Example: \"Authorization: Bearer {token}\"",
                        Name = "X-API-KEY",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey

                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apikey" }
                            },
                            new List<string>()
                        }
                    });
                }
                if (!string.IsNullOrEmpty(configuration[authorityKey]))
                {
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl =
                                    new Uri(
                                        $"{configuration[authorityKey]}authorize?audience={configuration[audienceKey]}"),
                                TokenUrl = new Uri($"{configuration[authorityKey]}token"),
                                Scopes = new Dictionary<string, string>()
                            }
                        }
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                            },
                            new List<string>()
                        }
                    });
                }
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public static void UseCustomSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IConfiguration configuration,
            string swaggerClientIdKey = "Auth:SwaggerClientId", bool disableHttps = false)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
                options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var prefix = disableHttps ? "http" : "https";
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer
                    {
                        Url =  $"{prefix}://{httpReq.Host.Value}{configuration["ASPNETCORE_BASEPATH"]}"
                    } };
                });
            });
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(
                options =>
                {
                    options.ShowExtensions();

                    if (!string.IsNullOrWhiteSpace(swaggerClientIdKey))
                    {
                        options.OAuthClientId(configuration[swaggerClientIdKey]);
                    }

                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion))
                    {
                        options.SwaggerEndpoint($"{configuration["ASPNETCORE_BASEPATH"]}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        options.RoutePrefix = string.Empty;
                    }
                    options.DisplayRequestDuration();
                    options.DisplayOperationId();
                });
        }

        private static string XmlCommentsFilePath(string assemblyName)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = assemblyName + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
