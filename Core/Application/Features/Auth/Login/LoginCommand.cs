// <copyright file="LoginCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Login
{
    public class LoginCommand
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
