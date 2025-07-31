// <copyright file="SerilogConfiguration.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Utils.Logging
{
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public static class SerilogConfiguration
    {
        public static void UseSerilogLogger(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
        }
    }
}
