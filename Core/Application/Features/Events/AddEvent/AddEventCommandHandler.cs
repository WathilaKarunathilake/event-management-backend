// <copyright file="AddEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Events.AddEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Features.Images.ImageUpload;
    using EventManagementAPI.Core.Application.Features.Notification.NotificationSendCommand;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Enums;
    using EventManagementAPI.Core.Domain.Errors;
    using EventManagementAPI.Infrastructure.Notification.Enums;
    using EventManagementAPI.Infrastructure.Notification.Models;
    using MediatR;

    public class AddEventCommandHandler : ICommandHandler<AddEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly ISender sender;
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AddEventCommandHandler(IRepository<Event> eventRepository, ISender sender, IUserService userService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.sender = sender;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<string>> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await this.unitOfWork.BeginTransactionAsync();

                var userIdString = request.User!.GetUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Result<string>.Failure(DomainErrors.Auth.NotAuthenticated());
                }

                request.CreatedBy = userId;

                var user = await this.userService.GetUserDetailsByIdAsync(request.CreatedBy);
                if (user == null)
                {
                    return Result<string>.Failure(DomainErrors.Auth.UserNotFound(request.CreatedBy));
                }

                var creatorName = user.Name;
                var eventEntity = this.mapper.Map<Event>(request);
                eventEntity.CreatorName = creatorName;

                await this.eventRepository.AddAsync(eventEntity);
                await this.unitOfWork.SaveChangesAsync(cancellationToken);

                if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    var uploadResult = await this.sender.Send(new ImageUploadCommand
                    {
                        ImageName = eventEntity.Id.ToString(),
                        ImageUrl = request.ImageUrl,
                    });

                    if (uploadResult.IsSuccess)
                    {
                        eventEntity.ImageUrl = uploadResult.Value;
                        await this.eventRepository.UpdateAsync(eventEntity);
                        await this.unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }

                var inApp = new NotificationMessage
                {
                    Type = NotificationType.Inapp,
                    Subject = "New event added !",
                    Content = $"Event {request.Title} just added",
                    Recipients = new List<string> { UserRole.PUBLICUSER.ToString() },
                };
                await this.sender.Send(new NotificationSendCommand { NotificationMessage = inApp });
                await this.unitOfWork.CommitAsync();

                return Result<string>.Success("Event added successfully !");
            }
            catch (Exception)
            {
                await this.unitOfWork.RollbackAsync();
                return Result<string>.Failure(DomainErrors.Transaction.TransactionFailed());
            }
        }
    }
}
