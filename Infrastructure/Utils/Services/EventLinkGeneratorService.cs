// <copyright file="EventLinkGeneratorService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Utils.Services
{
    using EventManagementAPI.Core.Application.Contracts.Utilities;
    using Microsoft.Extensions.Configuration;

    public class EventLinkGeneratorService : IEventLinkGeneratorService
    {
        private readonly string frontendOrigin;

        public EventLinkGeneratorService(IConfiguration configuration)
        {
            this.frontendOrigin = configuration["Frontend:Origin"] !;
        }

        public string GenerateEventUrl(string title, string id, DateTime start, DateTime end, string location)
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["title"] = title,
                ["refId"] = id,
                ["date"] = start.ToString("yyyy-MM-dd"),
                ["start"] = start.ToString("HH:mm"),
                ["end"] = end.ToString("HH:mm"),
                ["venue"] = location,
            };

            string queryString = string.Join("&", queryParams
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value!)}"));

            return $"{this.frontendOrigin}/event-info?{queryString}";
        }
    }
}
