// <copyright file="AuthEndpoints.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Endpoints
{
    using EventManagementAPI.API.Extensions;
    using EventManagementAPI.Core.Application.Features.Auth.Login;
    using EventManagementAPI.Core.Application.Features.Auth.Register;
    using EventManagementAPI.Core.Application.Response;
    using MediatR;

    public class AuthEndpoints : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var authGroup = app.MapGroup("/api/auth").WithTags("Auth Endpoints");

            authGroup.MapPost("/login", LoginUser);
            authGroup.MapPost("/register", RegisterUser);
        }

        private static async Task<IResult> RegisterUser(RegistrationCommand command, ISender sender)
        {
            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> LoginUser(LoginCommand command, ISender sender)
        {
            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }
    }
}
