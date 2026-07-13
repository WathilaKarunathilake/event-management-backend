// <copyright file="IImageUploadService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Utilities
{
    public interface IIMageUploadService
    {
        Task<string> UploadFileAsync(byte[] fileData, string fileName);

        Task DeleteFileAsync(string fileName);
    }
}
