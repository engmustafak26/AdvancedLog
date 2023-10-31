using AdvancedLog.Abstraction;
using AdvancedLog.Configurations;
using AdvancedLog.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedLog.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddAdvancedLog(this IServiceCollection services, IConfiguration configuration, Func<AdvancedLogConfiguration> configFunc = null)
        {
            var logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .Enrich.FromLogContext()
                            .CreateLogger();


            if (configFunc == null)
            {
                services.AddScoped<IExecutionContext, ExecutionContext>();
            }
            else
            {
                services.AddScoped<IExecutionContext>(service=>configFunc()?.ExecutionContext);
            }

            services.AddScoped(typeof(IAppLogger<>),typeof(FileLogger<>));
            services.AddLogging(builder => builder.AddSerilog(logger, dispose: true));
        }
    }
}
