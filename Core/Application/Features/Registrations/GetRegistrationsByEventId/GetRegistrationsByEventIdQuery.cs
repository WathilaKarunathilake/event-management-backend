// <copyright file="GetRegistrationsByEventIdQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.GetRegistrations
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetRegistrationsByEventIdQuery : IQuery<Result<List<UserDataDTO>>>
    {
        public Guid EventId { get; set; }
    }
}
