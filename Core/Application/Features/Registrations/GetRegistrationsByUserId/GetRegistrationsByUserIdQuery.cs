// <copyright file="GetRegistrationsByIdQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;

namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrationsById
{
    public class GetRegistrationsByUserIdQuery : IQuery<Result<List<EventDTO>>>
    {
        public Guid UserId { get; set; }
    }
}
