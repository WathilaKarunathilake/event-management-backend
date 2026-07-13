// <copyright file="RegisteredUsersDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.DTO
{
    using EventManagementAPI.Core.Domain.Enums;

    public class RegisteredUsersDTO : UserDataDTO
    {
        public string? AccountName { get; set; }

        public string? AccountEmail { get; set; }

        public string? AccountPhoneNumber { get; set; }

        public RegisterType RegisterType { get; set; }
    }
}
