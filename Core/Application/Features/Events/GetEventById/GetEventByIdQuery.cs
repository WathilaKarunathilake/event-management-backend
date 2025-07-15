using EventManagementAPI.Core.Application.Contracts.Messaging.Query;
using EventManagementAPI.Core.Application.DTO;
using EventManagementAPI.Core.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.Core.Application.Features.Events.GetEventById
{
    public class GetEventByIdQuery : IQuery<Result<EventDTO>>
    {
        public Guid Id { get; set; }
    }
}
