// <copyright file="UpdateEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.UpdateEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Features.Images.ImageUpload;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Errors;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly ILogger<UpdateEventCommandHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISender sender;
        private readonly IMapper mapper;

        public UpdateEventCommandHandler(IRepository<Event> eventRepository, ILogger<UpdateEventCommandHandler> logger, IUnitOfWork unitOfWork, ISender sender, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.sender = sender;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await this.unitOfWork.BeginTransactionAsync();

                var eventDetails = await this.eventRepository.GetByIdAsync(request.Id);
                if (eventDetails == null)
                {
                    return Result<string>.Failure(DomainErrors.Event.NotFound(request.Id));
                }

                this.mapper.Map(request, eventDetails);

                if (!string.IsNullOrEmpty(request.ImageUrl) && !request.ImageUrl.StartsWith("http"))
                {
                    var result = await this.sender.Send(new ImageUploadCommand
                    {
                        ImageName = eventDetails.Id.ToString(),
                        ImageUrl = request.ImageUrl,
                    });

                    if (result.IsSuccess)
                    {
                        eventDetails.ImageUrl = result.Value;
                    }
                }

                await this.eventRepository.UpdateAsync(eventDetails);
                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                await this.unitOfWork.CommitAsync();

                return Result<string>.Success("Event updated successfully");
            }
            catch (Exception)
            {
                await this.unitOfWork.RollbackAsync();
                return Result<string>.Failure(DomainErrors.Transaction.TransactionFailed());
            }
        }
    }
}
