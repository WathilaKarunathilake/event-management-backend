// <copyright file="UserDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.DTO
{
    using EventManagementAPI.Core.Domain.Enums;

    public class UserDTO
    {
        public bool Succeeded { get; set; }

        public string? Name { get; set; }

        public string? Errors { get; set; }

        public Guid UserId { get; set; }

        public UserRole? Role { get; set; }

        public string? Email { get; set; }
    }
}
