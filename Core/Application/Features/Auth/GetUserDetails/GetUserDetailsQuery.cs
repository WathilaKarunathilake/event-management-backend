// <copyright file="GetUserDetailsQuery.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.GetUserDetails
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using System.Security.Claims;

    public class GetUserDetailsQuery : IQuery<Result<UserDataDTO>>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
