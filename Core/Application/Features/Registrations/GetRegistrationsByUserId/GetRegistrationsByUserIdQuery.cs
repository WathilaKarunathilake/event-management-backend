// <copyright file="GetRegistrationsByUserIdQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrationsById
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetRegistrationsByUserIdQuery : IQuery<Result<List<EventDTO>>>
    {
        public Guid UserId { get; set; }
    }
}
