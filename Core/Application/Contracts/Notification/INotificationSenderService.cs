// <copyright file="INotificationSenderService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Notification
{
    using EventManagementAPI.Core.Application.DTO;

    public interface INotificationSenderService
    {
        Task SendAsync(NotificationMessageDTO message, CancellationToken cancellationToken = default);
    }
}
