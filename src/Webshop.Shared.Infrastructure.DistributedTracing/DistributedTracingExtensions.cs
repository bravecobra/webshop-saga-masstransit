using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Webshop.Shared.Infrastructure.DistributedTracing
{
    /// <summary>
    /// 
    /// </summary>
    public static class DistributedTracingExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedTracing(this IServiceCollection services,
            IConfiguration configuration)
        {
            var exporter = configuration.GetValue<string>("DistributedTracing:UseExporter").ToLowerInvariant();
            switch (exporter)
            {
                case "jaeger":
                    services.AddOpenTelemetryTracing((builder) => builder
                        .SetResource(Resources.CreateServiceResource(configuration.GetValue<string>("DistributedTracing:Jaeger:ServiceName")))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation(options =>
                        {
                            options.SetHttpFlavor = true;
                        })
                        .AddSqlClientInstrumentation(options =>
                        {
                            options.SetTextCommandContent = true;
                        })
                        .AddEntityFrameworkCoreInstrumentation(options =>
                        {
                            options.SetTextCommandContent = true;
                        })
                        
                        .AddJaegerExporter(jaegerOptions =>
                        {
                            jaegerOptions.AgentHost = configuration.GetValue<string>("DistributedTracing:Jaeger:Host");
                            jaegerOptions.AgentPort = configuration.GetValue<int>("DistributedTracing:Jaeger:Port");
                        }));
                    break;
                // case "zipkin":
                //     services.AddOpenTelemetryTracing((builder) => builder
                //         .AddAspNetCoreInstrumentation()
                //         .AddHttpClientInstrumentation()
                //         .AddZipkinExporter(zipkinOptions =>
                //         {
                //             zipkinOptions.ServiceName = this.Configuration.GetValue<string>("Zipkin:ServiceName");
                //             zipkinOptions.Endpoint = new Uri(this.Configuration.GetValue<string>("Zipkin:Endpoint"));
                //         }));
                //     break;
                // case "otlp":
                //     services.AddOpenTelemetryTracing((builder) => builder
                //         .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(this.Configuration.GetValue<string>("Otlp:ServiceName")))
                //         .AddAspNetCoreInstrumentation()
                //         .AddHttpClientInstrumentation()
                //         .AddOtlpExporter(otlpOptions =>
                //         {
                //             otlpOptions.Endpoint = this.Configuration.GetValue<string>("Otlp:Endpoint");
                //         }));
                //     break;
                default:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter());
                    break;

            }
            return services;
        }
    }
}
