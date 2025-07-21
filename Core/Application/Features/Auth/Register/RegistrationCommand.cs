// <copyright file="RegistrationCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Auth.Register
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Enums;

    public class RegistrationCommand : ICommand<Result<AuthDTO>>
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Name { get; set; }

        public UserRole Role { get; set; }
    }
}
