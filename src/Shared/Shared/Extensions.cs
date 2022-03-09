using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Prodigy;
using Prodigy.HTTP;
using Prodigy.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace Shared
{
    public static  class Extensions
    {
        public const string CorrelationHeaderKey = "CorrelationId";

        public static WebApplicationBuilder AddCore(this WebApplicationBuilder builder)
        {
            builder.WebHost.UseLogging((ctx, srv, cfg) =>
            {
                cfg.Enrich.WithCorrelationIdHeader(CorrelationHeaderKey);
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddProdigy();
            builder.Services.AddCustomHttpClient();
            
            return builder;
        }

        private static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
        {
            var options = services.GetSettings<HttpClientOptions>("httpClient");
            services.AddSingleton(options);
            services.AddHttpClient<IHttpClient, CustomHttpClient>();

            return services;
        }

    }

}
