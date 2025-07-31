// <copyright file="EmailSendingService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification.Channels
{
    using System.Text;
    using System.Text.Json;
    using EventManagementAPI.Infrastructure.Notification.Contracts;
    using EventManagementAPI.Infrastructure.Notification.Enums;
    using EventManagementAPI.Infrastructure.Notification.Models;
    using Microsoft.Extensions.Configuration;

    public class EmailSendingService : INotificationChannel
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly string senderEmail;
        private readonly string senderName;

        public EmailSendingService(HttpClient httpClient, IConfiguration config)
        {
            this.httpClient = httpClient;
            this.apiKey = config["Brevo:ApiKey"] ?? throw new ArgumentNullException("Brevo:ApiKey is missing in configuration.");
            this.senderEmail = config["Brevo:SenderEmail"] ?? throw new ArgumentNullException("Brevo:SenderEmail is missing in configuration.");
            this.senderName = config["Brevo:SenderName"] ?? "EventMS"; // fallback name
        }

        public NotificationType Type => NotificationType.Email;

        public async Task SendAsync(NotificationMessage message, CancellationToken cancellationToken = default)
        {
            if (message.Type != NotificationType.Email)
            {
                throw new NotSupportedException("EmailSendingService only supports Email notifications.");
            }

            var emailRequest = new
            {
                sender = new { name = this.senderName, email = this.senderEmail },
                to = message.Recipients.Select(r => new { email = r }).ToList(),
                subject = message.Subject,
                htmlContent = message.Content,
            };

            var json = JsonSerializer.Serialize(emailRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
            {
                Content = content,
            };

            request.Headers.Add("API-key", this.apiKey);

            var response = await this.httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Brevo API call failed: {response.StatusCode} - {errorContent}");
            }
        }
    }
}
