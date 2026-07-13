// <copyright file="RegisteredEventsDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.DTO
{
    using EventManagementAPI.Core.Domain.Enums;

    public class RegisteredEventsDTO : EventDTO
    {
        public RegisterType RegisterType { get; set; }
    }
}
