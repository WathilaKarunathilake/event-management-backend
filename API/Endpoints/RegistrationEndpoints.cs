// <copyright file="RegistrationEndpoints.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Endpoints
{
    using EventManagementAPI.API.Extensions;
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

            registrationGroup.MapPost("/", RegisterEvent);
            registrationGroup.MapPut("/{id:guid}", CancelEventRegistration);
            registrationGroup.MapGet("/event/{id:guid}", GetRegistrationsByEventId);
            registrationGroup.MapGet("/user/{id:guid}", GetRegistrationsUserId);
        }

        private static async Task<IResult> RegisterEvent(RegisterEventCommand command, ISender sender)
        {
            var result = await sender.Send(command);
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> CancelEventRegistration(Guid id, ISender sender)
        {
            var result = await sender.Send(new GetRegistrationsByEventIdQuery { EventId = id });
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

        private static async Task<IResult> GetRegistrationsUserId(Guid id, ISender sender)
        {
            var result = await sender.Send(new GetRegistrationsByUserIdQuery { UserId = id });
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }
    }
}
