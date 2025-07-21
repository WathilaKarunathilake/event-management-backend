// <copyright file="RegisteredUsersDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Domain.Enums;

namespace EventManagementAPI.Core.Application.DTO
{
    public class RegisteredUsersDTO : UserDataDTO
    {
        public string? AccountName { get; set; }

        public string? AccountEmail { get; set; }

        public string? AccountPhoneNumber { get; set; }

        public RegisterType RegisterType { get; set; }
    }
}
