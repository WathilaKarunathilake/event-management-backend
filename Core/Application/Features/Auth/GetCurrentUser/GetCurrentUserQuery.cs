// <copyright file="GetCurrentUserQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;
using System.Security.Claims;

namespace EventManagementAPI.Core.Application.Features.Auth.GetCurrentUser
{
    public class GetCurrentUserQuery : IQuery<Result<CurrentUserDTO>>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
