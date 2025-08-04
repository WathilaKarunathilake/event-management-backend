// <copyright file="RegisterEventCommandHandler.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Features.Registrations.RegisterEvent
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.Contracts.Identity;
    using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Core.Application.Contracts.Utilities;
    using EventManagementAPI.Core.Application.Extensions;
    using EventManagementAPI.Core.Application.Features.Notification.NotificationSendCommand;
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Application.Templates;
    using EventManagementAPI.Core.Domain.Entities;
    using EventManagementAPI.Core.Domain.Enums;
    using EventManagementAPI.Core.Domain.Errors;
    using EventManagementAPI.Infrastructure.Notification.Enums;
    using EventManagementAPI.Infrastructure.Notification.Models;
    using MediatR;

    public class RegisterEventCommandHandler : ICommandHandler<RegisterEventCommand, Result<string>>
    {
        private readonly IRepository<Event> eventRepository;
        private readonly IRepository<Registration> registrationRepository;
        private readonly IQRCodeGeneratorSerivice qRCodeGeneratorSerivice;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISender sender;
        private readonly IIMageUploadService iMageUploadService;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public RegisterEventCommandHandler(IRepository<Event> eventRepository, IRepository<Registration> registrationRepository, IQRCodeGeneratorSerivice qRCodeGeneratorSerivice, IUnitOfWork unitOfWork, ISender sender, IIMageUploadService iMageUploadService, IMapper mapper, IUserService userService)
        {
            this.eventRepository = eventRepository;
            this.registrationRepository = registrationRepository;
            this.qRCodeGeneratorSerivice = qRCodeGeneratorSerivice;
            this.unitOfWork = unitOfWork;
            this.sender = sender;
            this.iMageUploadService = iMageUploadService;
            this.mapper = mapper;
            this.userService = userService;
        }

        public async Task<Result<string>> Handle(RegisterEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userIdString = request.User!.GetUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Result<string>.Failure(DomainErrors.Auth.NotAuthenticated());
                }

                request.UserId = userId;
                await this.unitOfWork.BeginTransactionAsync();

                var userDetails = await this.userService.GetUserDetailsByIdAsync(request.UserId);
                var evt = await this.eventRepository.GetByIdAsync(request.EventId);

                if (evt is null)
                {
                    return Result<string>.Failure(DomainErrors.Event.NotFound(request.EventId));
                }

                var allRegistrations = await this.registrationRepository.FindAllAsync(r =>
                    r.EventId == request.EventId &&
                    r.RegisterType == RegisterType.REGISTERED);

                if (allRegistrations.Count() >= evt.Capacity)
                {
                    return Result<string>.Failure(DomainErrors.Registration.EventIsFull(evt.Title!));
                }

                if (DateTime.UtcNow > evt.StartDateTime)
                {
                    return Result<string>.Failure(DomainErrors.Registration.CutoffPassed(evt.Title!));
                }

                var allMatching = await this.registrationRepository.FindAllAsync(r =>
                    r.EventId == request.EventId &&
                    r.UserId == request.UserId &&
                    r.RegisterType == RegisterType.REGISTERED);

                if (allMatching.Any())
                {
                    return Result<string>.Failure(DomainErrors.Registration.AlreadyRegistered(evt.Title!));
                }

                evt.TotalRegistrations++;
                bool isNowFull = evt.TotalRegistrations >= evt.Capacity;

                var registration = this.mapper.Map<Registration>(request);
                await this.eventRepository.UpdateAsync(evt);
                await this.registrationRepository.AddAsync(registration);

                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                await this.unitOfWork.CommitAsync();

                string qrCodeText = $"""
                    Event: {evt.Title}
                    Reference ID: {evt.Id}
                    Date: {evt.StartDateTime:yyyy-MM-dd}
                    Time: {evt.StartDateTime:HH:mm} - {evt.EndDateTime:HH:mm} UTC
                    Venue: {evt.Location}
                """.ReplaceLineEndings("\n");

                var fileName = Guid.NewGuid().ToString() + ".jpg";
                var imageUrl = await this.iMageUploadService.UploadFileAsync(this.qRCodeGeneratorSerivice.GenerateQrCodeImageArr(qrCodeText), fileName);

                var recipientEmails = new List<string> { request.Email!, userDetails!.Email! }
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                var email = new NotificationMessage
                {
                    Type = NotificationType.Email,
                    Subject = $"Thank you for registering - {evt.Title} !",
                    Content = RegistrationEmailTemplate.Generate(evt, userDetails!, imageUrl),
                    Recipients = recipientEmails,
                };
                await this.sender.Send(new NotificationSendCommand { NotificationMessage = email });

                if (isNowFull)
                {
                    var inAppNotification = new NotificationMessage
                    {
                        Type = NotificationType.Inapp,
                        Subject = $"Maximum capacity reached !",
                        Content = $"Maximum capacity reached for event {evt.Title}",
                        Recipients = new List<string> { UserRole.ADMIN.ToString() },
                    };

                    await this.sender.Send(new NotificationSendCommand { NotificationMessage = inAppNotification });
                }

                return Result<string>.Success("Registered successfully.");
            }
            catch (Exception)
            {
                await this.unitOfWork.RollbackAsync();
                return Result<string>.Failure(DomainErrors.Transaction.TransactionFailed());
            }
        }
    }
}
