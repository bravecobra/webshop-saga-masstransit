using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace Webshop.Shared.Infrastructure.Versioning
{
    /// <summary>
    ///     Extensions for the default Farmad implementation api versioning on the <see cref="IServiceCollection" />. 
    /// </summary>
    public static class ConfigureApiVersioningExtensions
    {
        /// <summary>
        ///     Adds service API versioning and an API explorer that is API version aware to the specified services collection.
        ///     Will search for the latest version determined by classes that are decorated with the <see cref="ApiVersionAttribute"/> attribute.
        ///     When no assembly is specified, the calling assembly is used.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            var latestVersion = assemblies.SelectMany(assembly => assembly.GetTypes())
                .Select(type => type.GetCustomAttribute<ApiVersionAttribute>(true))
                .Where(attribute => attribute != null)
                .SelectMany(attribute => attribute!.Versions)
                .OrderByDescending(version => version)
                .FirstOrDefault();

            if (latestVersion == null)
            {
                throw new InvalidOperationException("Could not find any class that was decorated with the ApiVersion attribute.");
            }

            services.AddCustomApiVersioning(latestVersion);

            return services;
        }

        /// <summary>
        ///     Adds service API versioning and an API explorer that is API version aware to the specified services collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="defaultVersion">The default API version applied to the services that do not have explicit versions.</param>
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services, ApiVersion defaultVersion)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.UseApiBehavior = false;
                    options.DefaultApiVersion = defaultVersion;
                    options.AssumeDefaultVersionWhenUnspecified = false;
                    options.Conventions.Add(new VersionByNamespaceConvention());
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new QueryStringApiVersionReader(),
                        new HeaderApiVersionReader("api-version"));

                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.DefaultApiVersion = defaultVersion;
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";
                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            return services;
        }
    }
}
