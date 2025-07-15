using EventManagementAPI.Core.Application.Contracts.Messaging.Commands;
using EventManagementAPI.Core.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventManagementAPI.Core.Application.Features.Events.DeleteEvent
{
    public class DeleteEventCommand : ICommand<Result<string>>
    {
        public Guid Id { get; set; }
    }
}
