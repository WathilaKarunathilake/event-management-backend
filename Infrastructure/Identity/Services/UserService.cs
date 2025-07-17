// <copyright file="UserService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using EventManagementAPI.Core.Application.Contracts.Identity;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Domain.Enums;
using EventManagementAPI.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EventManagementAPI.Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task AddToRoleAsync(string email, string role)
        {
            var users = await this.userManager.Users.Where(u => u.Email == email).ToListAsync();
            var user = users.FirstOrDefault();
            if (user != null)
            {
                await this.userManager.AddToRoleAsync(user, role);
            }
        }

        public async Task<bool> CheckPasswordAsync(string email, string password)
        {
            var users = await this.userManager.Users.Where(u => u.Email == email).ToListAsync();
            var user = users.FirstOrDefault();
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
                return new UserDTO { Succeeded = false, Errors = "User already exists with email" };
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Name = username,
                Email = email,
                PhoneNumber = phoneNumber,
            };

            var result = await this.userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                string errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                return new UserDTO { Succeeded = false, Errors = errorMessages };
            }

            var roleResult = await this.userManager.AddToRoleAsync(user, role.ToString());
            if (!roleResult.Succeeded)
            {
                string roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                return new UserDTO
                {
                    Errors = $"User created, but failed to assign role: {roleErrors}",
                    Succeeded = false,
                };
            }

            return new UserDTO
            {
                Name = username,
                Email = email,
                Role = role,
                Succeeded = true,
                UserId = Guid.Parse(user.Id),
            };
        }

        public async Task<string> GetEmailFromId(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            return user.Email;
        }

        public async Task<UserDTO> GetUserDetailsFromEmail(string email)
        {
            var users = await this.userManager.Users.Where(u => u.Email == email).ToListAsync();
            var existingUser = users.FirstOrDefault();
            if (existingUser == null)
            {
                return new UserDTO
                {
                    Errors = "User not found.",
                    Succeeded = false,
                    UserId = Guid.Empty,
                };
            }

            var roles = await this.userManager.GetRolesAsync(existingUser);

            UserRole? parsedRole = null;
            if (Enum.TryParse<UserRole>(roles.FirstOrDefault(), ignoreCase: true, out var roleEnum))
            {
                parsedRole = roleEnum;
            }

            return new UserDTO
            {
                Name = existingUser.Name,
                UserId = Guid.Parse(existingUser.Id),
                Email = existingUser.Email,
                Role = parsedRole,
            };
        }

        public async Task<List<UserDataDTO>> GetUsersByIdsAsync(List<Guid> userIds)
        {
            var userIdStrings = userIds.Select(id => id.ToString()).ToList();

            var users = await userManager.Users
                .Where(u => userIdStrings.Contains(u.Id))
                .ToListAsync();

            var userDataList = users.Select(u => new UserDataDTO
            {
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
            }).ToList();

            return userDataList;
        }

        public async Task<UserDataDTO?> GetUserDetailsByIdAsync(Guid userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return null;
            }

            var userData = new UserDataDTO
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return userData;
        }
    }
}
