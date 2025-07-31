// <copyright file="ImageUploadCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Images.ImageUpload
{
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Utilities;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

    public class ImageUploadCommandHandler : ICommandHandler<ImageUploadCommand, Result<string>>
    {
        private readonly IIMageUploadService iMageUpload;

        public ImageUploadCommandHandler(IIMageUploadService iMageUpload)
        {
            this.iMageUpload = iMageUpload;
        }

        public async Task<Result<string>> Handle(ImageUploadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(request.ImageUrl!);
                var fileName = request.ImageName + ".jpg";
                var imageUrl = await this.iMageUpload.UploadFileAsync(imageBytes, fileName);
                return Result<string>.Success(imageUrl);
            }
            catch (Exception e)
            {
                return Result<string>.Failure(DomainErrors.Custom.Failure(e.Message));
            }
        }
    }
}
