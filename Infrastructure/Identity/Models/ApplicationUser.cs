// <copyright file="ApplicationUser.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Identity.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public void SetRefreshToken(string token, DateTime expiry)
        {
            this.RefreshToken = token;
            this.RefreshTokenExpiryTime = expiry;
        }
    }
}
