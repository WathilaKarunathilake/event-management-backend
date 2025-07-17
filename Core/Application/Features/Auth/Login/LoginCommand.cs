// <copyright file="LoginCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Login
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;

    public class LoginCommand : ICommand<Result<AuthDTO>>
    {
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
