using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.Response;

namespace EventManagementAPI.Core.Application.Features.Events.DeleteEvent
{
    public class DeleteEventCommand : ICommand<Result<string>>
    {
        public Guid Id { get; set; }
    }
}
