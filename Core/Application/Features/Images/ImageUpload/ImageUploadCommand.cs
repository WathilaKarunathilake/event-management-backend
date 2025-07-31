// <copyright file="ImageUploadCommand.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Images.ImageUpload
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Response;

    public class ImageUploadCommand : ICommand<Result<string>>
    {
        public string? ImageName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
