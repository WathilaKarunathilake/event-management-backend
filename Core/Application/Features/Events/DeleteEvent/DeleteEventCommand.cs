// <copyright file="DeleteEventCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.DeleteEvent
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;

    public class DeleteEventCommand : ICommand<Result<string>>
    {
        public Guid Id { get; set; }
    }
}
