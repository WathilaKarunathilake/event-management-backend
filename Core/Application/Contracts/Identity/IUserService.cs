// <copyright file="IUserService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Domain.Enums;

namespace EventManagementAPI.Core.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(string email, string password);

        Task AddToRoleAsync(string email, string role);

        Task<UserDTO> GetUserDetailsFromEmail(string email);

        Task<string> GetEmailFromId(string id);

        Task<UserDTO> CreateUserAsync(string username, string email, string password, string phoneNumber, UserRole role);
    }
}
