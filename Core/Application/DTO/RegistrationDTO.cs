// <copyright file="RegistrationDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.DTO
{
    public class RegistrationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int capacity { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}
