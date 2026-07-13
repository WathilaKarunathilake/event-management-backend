// <copyright file="ClaimsPrincipalExtensions.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Extensions
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user?.FindFirst("name")?.Value ?? string.Empty;
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        public static bool IsAuthenticated(this ClaimsPrincipal user)
        {
            return user?.Identity?.IsAuthenticated ?? false;
        }
    }
}
