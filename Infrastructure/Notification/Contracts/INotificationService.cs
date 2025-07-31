// <copyright file="INotificationService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification.Contracts
{
    using EventManagementAPI.Infrastructure.Notification.Models;

    public interface INotificationService
    {
        Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    }
}
