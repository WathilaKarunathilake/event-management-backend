// <copyright file="GetCurrentUserQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.GetCurrentUser
{
    using System.Security.Claims;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class GetCurrentUserQuery : IQuery<Result<CurrentUserDTO>>
    {
        public ClaimsPrincipal? User { get; set; }
    }
}
