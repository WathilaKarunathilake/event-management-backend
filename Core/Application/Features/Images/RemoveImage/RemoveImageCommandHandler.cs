// <copyright file="RemoveImageCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Images.RemoveImage
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Utilities;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;

    public class RemoveImageCommandHandler : ICommandHandler<RemoveImageCommand, Result<bool>>
    {
        private readonly IIMageUploadService iMageUpload;

        public RemoveImageCommandHandler(IIMageUploadService iMageUpload)
        {
            this.iMageUpload = iMageUpload;
        }

        public async Task<Result<bool>> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await this.iMageUpload.DeleteFileAsync(request.ImageName + ".jpg"!);
                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Failure(DomainErrors.Custom.Failure(e.Message));
            }
}
    }
}
