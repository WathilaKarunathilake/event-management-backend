// <copyright file="UserService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Identity.Services
{
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Domain.Enums;
    using EventManagementAPI.Infrastructure.Identity.Factories;
    using EventManagementAPI.Infrastructure.Identity.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task AddToRoleAsync(string email, string role)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                await this.userManager.AddToRoleAsync(user, role);
            }
        }

        public async Task<bool> CheckPasswordAsync(string email, string password)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            return await this.userManager.CheckPasswordAsync(user, password);
        }

        public async Task<UserDTO> CreateUserAsync(string username, string email, string password, string phoneNumber, UserRole role)
        {
            var existingUser = await this.userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return UserDTOFactory.Failed("User already exist with the email provided");
            }

            var user = UserFactory.Create(username, email, phoneNumber);
            var result = await this.userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                string errorMessage = result.Errors.Select(e => e.Description).FirstOrDefault() ?? "Unknown error";
                return UserDTOFactory.Failed(errorMessage);
            }

            var roleResult = await this.userManager.AddToRoleAsync(user, role.ToString());
            if (!roleResult.Succeeded)
            {
                string roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                return UserDTOFactory.Failed($"User created, but failed to assign role: {roleErrors}");
            }

            return UserDTOFactory.FromApplicationUser(user, role);
        }

        public async Task<string> GetEmailFromId(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            return user!.Email!;
        }

        public async Task<UserDTO> GetUserDetailsFromEmail(string email)
        {
            var existingUser = await this.userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser == null)
            {
                return UserDTOFactory.Failed("User not found.");
            }

            var roles = await this.userManager.GetRolesAsync(existingUser);
            return UserDTOFactory.FromApplicationUser(existingUser, roles.FirstOrDefault());
        }

        public async Task<List<UserDTO>> GetUsersByIdsAsync(List<Guid> userIds)
        {
            var userIdStrings = userIds.Select(id => id.ToString()).ToList();

            var users = await this.userManager.Users
                .Where(u => userIdStrings.Contains(u.Id))
                .ToListAsync();

            var userDtos = await Task.WhenAll(users.Select(async user =>
            {
                var roles = await this.userManager.GetRolesAsync(user);
                var primaryRole = roles.FirstOrDefault();
                return UserDTOFactory.FromApplicationUser(user, primaryRole);
            }));
            return userDtos.ToList();
        }

        public async Task<UserDTO?> GetUserDetailsByIdAsync(Guid userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return null;
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var userData = UserDTOFactory.FromApplicationUser(user, roles.FirstOrDefault());
            return userData;
        }

        public async Task SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.SetRefreshToken(refreshToken, expiryTime);
                await this.userManager.UpdateAsync(user);
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
