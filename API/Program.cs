// <copyright file="Program.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.API.Extensions;
using EventManagementAPI.Infrastructure.Identity.Extensions;
using EventManagementAPI.Infrastructure.Utils.Logging;
using WathilaKarunathilake.Notification.Hubs;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var frontendOrigin = builder.Configuration.GetValue<string>("Frontend:Origin") !;

        builder.Services
            .AddCorsPolicy(frontendOrigin)
            .AddDbContexts(builder.Configuration)
            .AddIdentityServices()
            .AddApplicationServices()
            .AddMediatRAndAutoMapper()
            .AddAuthenticationAndAuthorization(builder.Configuration)
            .AddOtherInfrastructure()
            .AddSwaggerConfiguration();

        builder.Host.UseSerilogLogger();

        var app = builder.Build();
        app.UseCors("AllowFrontend");

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.RegisterAllEndpointGroups();

        // Mapping the Notifcations connection
        app.MapHub<NotificationHub>("/notificationHub");

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedData.InitializeAsync(services);
        }

        app.Run();
    }
}
