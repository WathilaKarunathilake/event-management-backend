// <copyright file="RegisterEventCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;

    public class RegisterEventCommand : ICommand<Result<string>>
    {
        public Guid EventId { get; set; }

        public Guid UserId { get; set; }
    }
}
