// <copyright file="UserIdMatchRequirmentHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Identity.Requirements
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;

    public class UserIdMatchRequirmentHandler : AuthorizationHandler<UserIdMatchRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIdMatchRequirment requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                var routeId = httpContext.Request.RouteValues["id"]?.ToString();
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(routeId) && routeId == userId)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
