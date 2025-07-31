// <copyright file="IJwtParserService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Identity
{
    using System.Security.Claims;

    public interface IJwtParserService
    {
        string? ExtractUserId(string expiredToken);

        ClaimsPrincipal? ParseToken(string expiredToken);
    }
}
