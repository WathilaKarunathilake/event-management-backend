// <copyright file="RefreshTokenCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.RefreshToken
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class RefreshTokenCommand : ICommand<Result<AuthDTO>>
    {
        public string? JwtToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
