// <copyright file="JwtParserService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Identity.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class JwtParserService : IJwtParserService
    {
        private readonly IConfiguration config;

        public JwtParserService(IConfiguration config)
        {
            this.config = config;
        }

        public string? ExtractUserId(string expiredToken)
        {
            var principal = this.ParseToken(expiredToken);
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal?.FindFirst("sub")?.Value;
        }

        public ClaimsPrincipal? ParseToken(string expiredToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(this.config["JwtSettings:Key"] !);

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = this.config["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = this.config["JwtSettings:Audience"],
                ValidateLifetime = false,
            };

            try
            {
                return tokenHandler.ValidateToken(expiredToken, validationParams, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}
