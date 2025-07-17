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
    using EventManagementAPI.Core.Application.Features.Events.UpdateEvent;
    using EventManagementAPI.Core.Application.Response;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class EventEndpoints : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var eventGroup = app.MapGroup("/api/events").WithTags("Event Endpoints");

            eventGroup.MapPost("/", AddEvent);
            eventGroup.MapGet("/", GetEvents);
            eventGroup.MapGet("/{id:guid}", GetEventById);
            eventGroup.MapPut("/{id:guid}", UpdateEvent);
            eventGroup.MapDelete("/{id:guid}", DeleteEvent);
        }

        private static async Task<IResult> AddEvent(AddEventCommand command, ISender sender)
        {
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

        private static async Task<IResult> DeleteEvent(Guid id, ISender sender)
        {
            var result = await sender.Send(new DeleteEventCommand { Id = id });

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }
    }
}
