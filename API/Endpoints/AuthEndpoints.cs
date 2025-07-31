// <copyright file="AuthEndpoints.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Endpoints
{
    using System.Security.Claims;
    using EventManagementAPI.API.Extensions;
    using EventManagementAPI.Core.Application.Features.Auth.GetCurrentUser;
    using EventManagementAPI.Core.Application.Features.Auth.GetUserDetails;
    using EventManagementAPI.Core.Application.Features.Auth.Login;
    using EventManagementAPI.Core.Application.Features.Auth.RefreshToken;
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
            authGroup.MapPost("/refresh", RefreshToken);
            authGroup.MapPost("/logout", (Delegate)Logout);

            authGroup.MapGet("/info", GetUserInfo).RequireAuthorization();
            authGroup.MapGet("/me", GetUserInfoByToken).RequireAuthorization();
        }

        private static async Task<IResult> GetUserInfoByToken(ClaimsPrincipal claims, ISender sender)
        {
            var result = await sender.Send(new GetCurrentUserQuery { User = claims });

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static async Task<IResult> RefreshToken(ISender sender, HttpRequest request, HttpResponse response)
        {
            if (!request.Cookies.TryGetValue("jwt_token", out var jwtToken))
            {
                return Results.Unauthorized();
            }

            if (!request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new RefreshTokenCommand { JwtToken = jwtToken, RefreshToken = refreshToken });
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            response.SetJwtCookie(result.Value!.Token!, result.Value!.RefreshToken!, true);
            return Results.Ok(ApiResponse.Success("Refreshed successfully"));
        }

        private static async Task<IResult> RegisterUser(RegistrationCommand command, ISender sender, HttpResponse response)
        {
            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            response.SetJwtCookie(result.Value!.Token!, result.Value!.RefreshToken!, true);
            return Results.Ok(ApiResponse.Success("Logged in successfully"));
        }

        private static async Task<IResult> LoginUser(LoginCommand command, ISender sender, HttpResponse response)
        {
            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            response.SetJwtCookie(result.Value!.Token!, result.Value!.RefreshToken!, command.RememberMe);
            return Results.Ok(ApiResponse.Success("Logged in successfully"));
        }

        private static async Task<IResult> GetUserInfo(ClaimsPrincipal claims, ISender sender)
        {
            var result = await sender.Send(new GetUserDetailsQuery { User = claims });
            if (!result.IsSuccess)
            {
                return Results.BadRequest(ApiResponse.Fail(result.Error!));
            }

            return Results.Ok(ApiResponse.Success(result.Value));
        }

        private static Task<IResult> Logout(HttpContext context)
        {
            context.Response.Cookies.Delete("jwt_token");
            context.Response.Cookies.Delete("refresh_token");
            return Task.FromResult(Results.Ok(ApiResponse.Success("Logged out successfully")));
        }
    }
}
