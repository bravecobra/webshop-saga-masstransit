using System.Collections.Generic;
using AutoMapper;
using Catalog.Configuration.Services;
using Catalog.Persistence;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Webshop.Shared.Infrastructure.ErrorHandling;
using Webshop.Shared.Infrastructure.ErrorHandling.Exceptions;
using Webshop.Shared.Infrastructure.Swagger;
using Webshop.Shared.Infrastructure.Versioning;
using Webshop.Shared.Validation;

namespace Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCompositionRoot();
            services.AddCustomApiVersioning();
            services.AddDbContext<CatalogDbContext>(options =>
            {
                options
                    .EnableSensitiveDataLogging(false)
                    .EnableDetailedErrors(false)
                    .UseSqlServer(Configuration.GetConnectionString("CatalogDbContext"))
                        .EnableDetailedErrors(false);
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddControllers()
                .AddFluentValidation(fv => fv
                    .RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddCustomSwagger(Configuration, typeof(Startup).Assembly.GetName().Name);
            //services.AddHttpCacheHeaders();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<ErrorHandlingMiddleware>(new List<IExceptionHandler>()
            {
                new ConflictExceptionHandler(),
                new NotFoundExceptionHandler(),
                new ValidationExceptionHandler()
            });

            app.UseAuthorization();
            app.UseCustomSwagger(provider, Configuration);
            //app.UseHttpCacheHeaders();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
