// <copyright file="EventSummaryDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.DTO
{
    public class EventSummaryDTO
    {
        public int TotalEvents { get; set; }

        public int ActiveEvents { get; set; }

        public int FilledEvents { get; set; }

        public int UpcommingEvents { get; set; }

        public int TotalAttendees { get; set; }
    }
}
