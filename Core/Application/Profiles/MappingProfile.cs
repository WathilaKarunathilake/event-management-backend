// <copyright file="MappingProfile.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Profiles
{
    using AutoMapper;
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Application.Features.Events.AddEvent;
    using EventManagementAPI.Core.Application.Features.Events.UpdateEvent;
    using EventManagementAPI.Core.Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<AddEventCommand, Event>().ReverseMap();
            this.CreateMap<UpdateEventCommand, Event>().ReverseMap();
            this.CreateMap<Event, EventDTO>().ReverseMap();
            this.CreateMap<List<Event>, List<EventDTO>>().ReverseMap();
        }
    }
}
