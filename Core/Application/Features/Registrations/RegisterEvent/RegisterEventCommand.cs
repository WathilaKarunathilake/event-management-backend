// <copyright file="RegisterEventCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent
{
    using System.Security.Claims;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;

    public class RegisterEventCommand : ICommand<Result<string>>
    {
        public ClaimsPrincipal? User { get; set; }

        public Guid EventId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid UserId { get; set; }
    }
}
