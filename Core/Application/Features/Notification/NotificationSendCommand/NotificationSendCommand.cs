// <copyright file="NotificationSendCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Notification.NotificationSendCommand
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class NotificationSendCommand : ICommand<Result<bool>>
    {
        public NotificationMessageDTO? NotificationMessage { get; set; }
    }
}
