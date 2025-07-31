// <copyright file="ImageUploadService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Utils.Services
{
    using Azure.Storage.Blobs;
    using EventManagementAPI.Core.Application.Contracts.Utilities;
    using Microsoft.Extensions.Configuration;

    public class ImageUploadService : IIMageUploadService
    {
        private readonly BlobContainerClient containerClient;

        public ImageUploadService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            this.containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadFileAsync(byte[] fileData, string fileName)
        {
            var blobClient = this.containerClient.GetBlobClient(fileName);

            using var stream = new MemoryStream(fileData);
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var blobClient = this.containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
