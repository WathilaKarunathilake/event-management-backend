// <copyright file="NotificationSendCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Notification.NotificationSendCommand
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Infrastructure.Notification.Models;

    public class NotificationSendCommand : ICommand<Result<bool>>
    {
        public NotificationMessage? NotificationMessage { get; set; }
    }
}
