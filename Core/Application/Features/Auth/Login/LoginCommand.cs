// <copyright file="LoginCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;
using System.Windows.Input;

namespace EventManagementAPI.Core.Application.Features.Auth.Login
{
    public class LoginCommand : ICommand<Result<AuthDTO>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
