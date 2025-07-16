// <copyright file="RegistrationCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Register
{
    public class RegistrationCommand
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        //public UserRole role { get; set; }
    }
}
