// <copyright file="CancelEventCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.Response;

namespace EventManagementAPI.Core.Application.Features.Registrations.CancelEvent
{
    public class CancelEventCommand : ICommand<Result<string>>
    {
        public Guid EventId { get; set; }
    }
}
