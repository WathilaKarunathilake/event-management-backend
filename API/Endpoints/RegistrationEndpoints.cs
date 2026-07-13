// <copyright file="RegistrationEndpoints.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Endpoints
{
    using System.Security.Claims;
    using EventManagementAPI.API.Extensions;
    using EventManagementAPI.Core.Application.Features.Registrations.CancelEvent;
    using EventManagementAPI.Core.Application.Features.Registrations.GetRegistrations;
    using EventManagementAPI.Core.Application.Features.Registrations.GetRegistrationsById;
    using EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent;
    using EventManagementAPI.Core.Application.Response;
    using MediatR;

    public class RegistrationEndpoints : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var registrationGroup = app.MapGroup("/api/registrations").WithTags("Event Registrations Endpoints");

            registrationGroup.MapPost("/", RegisterEvent).RequireAuthorization("PUBLICUSER");
            registrationGroup.MapPut("/{id:guid}", CancelEventRegistration).RequireAuthorization("PUBLICUSER");

            registrationGroup.MapGet("/user", GetRegistrationsUserId).RequireAuthorization("PUBLICUSER");
            registrationGroup.MapGet("/event/{id:guid}", GetRegistrationsByEventId)
                             .RequireAuthorization("ADMIN");
        }

        private static async Task<IResult> RegisterEvent(RegisterEventCommand command, ClaimsPrincipal claims, ISender sender)
        {
            command.User = claims;

            var result = await sender.Send(command);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> CancelEventRegistration(Guid id, ClaimsPrincipal claims, ISender sender)
        {
            var result = await sender.Send(new CancelEventCommand
            {
                EventId = id,
                User = claims,
            });
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> GetRegistrationsByEventId(Guid id, ISender sender)
        {
            var result = await sender.Send(new GetRegistrationsByEventIdQuery { EventId = id });
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> GetRegistrationsUserId(ClaimsPrincipal claims, ISender sender)
        {
            var result = await sender.Send(new GetRegistrationsByUserIdQuery { User = claims });
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }
    }
}
