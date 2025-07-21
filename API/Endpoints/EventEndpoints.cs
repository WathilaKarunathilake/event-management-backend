// <copyright file="EventEndpoints.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Endpoints
{
    using EventManagementAPI.API.Extensions;
    using EventManagementAPI.Core.Application.Features.Events.AddEvent;
    using EventManagementAPI.Core.Application.Features.Events.DeleteEvent;
    using EventManagementAPI.Core.Application.Features.Events.GetEventById;
    using EventManagementAPI.Core.Application.Features.Events.GetEvents;
    using EventManagementAPI.Core.Application.Features.Events.GetEventsByUserId;
    using EventManagementAPI.Core.Application.Features.Events.UpdateEvent;
    using EventManagementAPI.Core.Application.Response;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using System.Security.Claims;

    public class EventEndpoints : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var eventGroup = app.MapGroup("/api/events")
                         .WithTags("Event Endpoints")
                         .RequireAuthorization();

            eventGroup.MapPost("/", AddEvent).RequireAuthorization("ADMIN");
            eventGroup.MapPut("/{id:guid}", UpdateEvent).RequireAuthorization("ADMIN");
            eventGroup.MapDelete("/{id:guid}", DeleteEvent).RequireAuthorization("ADMIN");
            eventGroup.MapGet("/user", GetEventsByUserId).RequireAuthorization("ADMIN");
            eventGroup.MapGet("/", GetEvents);
            eventGroup.MapGet("/{id:guid}", GetEventById);
        }

        private static async Task<IResult> GetEventsByUserId(ClaimsPrincipal claims, ISender sender)
        {
            var result = await sender.Send(new GetEventsByUserIdQuery { User = claims });

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> AddEvent(AddEventCommand command, ClaimsPrincipal claims, ISender sender)
        {
            command.User = claims; // Adding the user claims to get the ID
            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Created($"/api/events/{result.Value}", ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> GetEvents(ISender sender)
        {
            var result = await sender.Send(new GetEventsQuery());

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> GetEventById(Guid id, ISender sender)
        {
            var result = await sender.Send(new GetEventByIdQuery { Id = id });

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> UpdateEvent(Guid id, UpdateEventCommand command, ISender sender)
        {
            command.Id = id; // Adding the ID to the request body

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> DeleteEvent(ClaimsPrincipal claims, ISender sender)
        {
            var result = await sender.Send(new DeleteEventCommand { User = claims });

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }
    }
}
