// <copyright file="IUserService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Identity
{
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Domain.Enums;

    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(string email, string password);

        Task AddToRoleAsync(string email, string role);

        Task<UserDTO> GetUserDetailsFromEmail(string email);

        Task<string> GetEmailFromId(string id);

        Task<List<UserDataDTO>> GetUsersByIdsAsync(List<Guid> userIds);

        Task<UserDataDTO?> GetUserDetailsByIdAsync(Guid userId);

        Task<UserDTO> CreateUserAsync(string username, string email, string password, string phoneNumber, UserRole role);
    }
}
