// <copyright file="PageResultDTO.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.DTO
{
    public class PageResultDTO<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public int TotalCount { get; set; }
    }
}
