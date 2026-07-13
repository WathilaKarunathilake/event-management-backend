// <copyright file="IEventLinkGeneratorService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Utilities
{
    public interface IEventLinkGeneratorService
    {
        string GenerateEventUrl(string title, string id, DateTime start, DateTime end, string location);
    }
}
