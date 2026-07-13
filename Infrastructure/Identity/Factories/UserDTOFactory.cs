// <copyright file="UserDTOFactory.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Identity.Factories
{
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Domain.Enums;
    using EventManagementAPI.Infrastructure.Identity.Models;

    public static class UserDTOFactory
    {
        public static UserDTO FromApplicationUser(ApplicationUser user, string? roleString)
        {
            UserRole? role = Enum.TryParse<UserRole>(roleString, true, out var parsedRole) ? parsedRole : null;
            return FromApplicationUser(user, role);
        }

        public static UserDTO FromApplicationUser(ApplicationUser user, UserRole? role)
        {
            return new UserDTO
            {
                UserId = Guid.Parse(user.Id),
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = role,
                Succeeded = true,
            };
        }

        public static UserDTO Failed(string error)
        {
            return new UserDTO
            {
                Errors = error,
                Succeeded = false,
                UserId = Guid.Empty,
            };
        }
    }
}
