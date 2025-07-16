// <copyright file="IEndpointGroup.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Extensions
{
    public interface IEndpointGroup
    {
        void MapEndpoints(IEndpointRouteBuilder app);
    }
}
