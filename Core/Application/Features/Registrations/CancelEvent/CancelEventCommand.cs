// <copyright file="CancelEventCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.CancelEvent
{
    using System.Security.Claims;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;

    public class CancelEventCommand : ICommand<Result<string>>
    {
        public Guid EventId { get; set; }

        public Guid UserId { get; set; }

        public ClaimsPrincipal User { get; set; }
    }
}
