// <copyright file="NotificationHub.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Notification.Hubs
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var role = this.Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, role);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var role = this.Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, role);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
