// <copyright file="IJwtTokenGenerateService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Identity
{
    public interface IJwtTokenGenerateService
    {
        string GenerateToken(string name, string userId, string email, string role);
    }
}
