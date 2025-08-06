// <copyright file="RegistrationEmailTemplate.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Templates
{
    using EventManagementAPI.Core.Application.DTO;
    using EventManagementAPI.Core.Domain.Entities;

    public static class RegistrationEmailTemplate
    {
        public static string Generate(Event evt, UserDTO user, string qrCodeUrl)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows() ? "India Standard Time" : "Asia/Kolkata");

            var startLocal = TimeZoneInfo.ConvertTimeFromUtc(evt.StartDateTime.ToUniversalTime(), timeZone);
            var endLocal = TimeZoneInfo.ConvertTimeFromUtc(evt.EndDateTime.ToUniversalTime(), timeZone);

            string startDateStr = startLocal.ToString("dddd, MMMM dd, yyyy");
            string endDateStr = endLocal.ToString("dddd, MMMM dd, yyyy");

            string startTimeStr = startLocal.ToString("HH:mm");
            string endTimeStr = endLocal.ToString("HH:mm");
            string dateDisplay = startDateStr == endDateStr
        ? startDateStr
        : $"{startDateStr} - {endDateStr}";

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            background-color: #6b21a8; /* Tailwind's bg-purple-700 */
            color: white;
            padding: 20px;
            text-align: center;
            border-radius: 8px 8px 0 0;
        }}
        .content {{
            padding: 20px;
            background-color: white;
        }}
        .event-details {{
            background-color: #f8f9fa;
            padding: 15px;
            border-radius: 5px;
            margin: 15px 0;
        }}
        .qr-section {{
            text-align: center;
            margin: 20px 0;
        }}
        .qr-code-text {{
            white-space: pre-wrap;
            font-family: monospace;
            background-color: #eee;
            padding: 10px;
            border-radius: 5px;
            font-size: 14px;
            margin-top: 10px;
            display: inline-block;
            text-align: left;
        }}
        .footer {{
            text-align: center;
            padding: 15px;
            font-size: 12px;
            color: #666;
            background-color: #f1f1f1;
            border-radius: 0 0 8px 8px;
        }}

        .qr-img {{
            width: 100px;
            height: 100px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>Registration Confirmed!</h1>
        </div>
        
        <div class=""content"">
            <p>Dear {user.Name},</p>
            <p>Thank you for registering for <strong>{evt.Title}</strong>.</p>
            
            <div class=""event-details"">
                <h3>Event Details:</h3>
                <p><strong>Reference Number:</strong> {evt.Id}</p>
                <p><strong>Event:</strong> {evt.Title}</p>
                <p><strong>Description:</strong> {evt.Description}</p>
                <p><strong>Date:</strong> {dateDisplay}</p>
                <p><strong>Time:</strong> {startTimeStr} - {endTimeStr} (local time)</p>
                <p><strong>Location:</strong> {evt.Location}</p>
                <p><strong>Capacity:</strong> {evt.Capacity} attendees</p>
            </div>
            
            <div class=""event-details"">
                <h3>Your Registration:</h3>
                <p><strong>Name:</strong> {user.Name}</p>
                <p><strong>Email:</strong> {user.Email}</p>
            </div>

            <div class=""qr-section"">
                <h3>Your QR Code:</h3>
                <img class=""qr-img"" src=""{qrCodeUrl}"" alt=""Event QR Code"" />
            </div>
            
            <p>We look forward to seeing you at the event!</p>
        </div>
        
        <div class=""footer"">
            <p>&copy; {DateTime.UtcNow.Year} Event Management System</p>
            <p>This is an automated email, please do not reply.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
