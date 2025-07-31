// <copyright file="RemoveImageCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Images.RemoveImage
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;

    public class RemoveImageCommand : ICommand<Result<bool>>
    {
        public string? ImageName { get; set; }
    }
}
