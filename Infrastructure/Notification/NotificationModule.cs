// <copyright file="NotificationModule.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification
{
    using EventManagementAPI.Infrastructure.Notification.Channels;
    using EventManagementAPI.Infrastructure.Notification.Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public static class NotificationModule
    {
        public static IServiceCollection AddNotificationModule(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddScoped<INotificationService, NotificationManager>();
            services.AddScoped<INotificationChannel, InAppNotificationSendingService>();

            services.AddHttpClient<INotificationChannel, EmailSendingService>();
            return services;
        }
    }
}
