// <copyright file="ICommand{TResponse}.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Messaging.Commands
{
    using MediatR;

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
