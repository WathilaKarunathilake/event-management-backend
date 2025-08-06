// <copyright file="UserFactory.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Identity.Factories
{
    using EventManagementAPI.Infrastructure.Identity.Models;

    public static class UserFactory
    {
        public static ApplicationUser Create(string username, string email, string phoneNumber)
        {
            return new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = username,
                PhoneNumber = phoneNumber,
            };
        }
    }
}
