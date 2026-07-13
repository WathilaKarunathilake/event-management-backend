// <copyright file="LoggingBehavior{TRequest,TResponse}.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Behaviours
{
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                var response = await next();
                this.logger.LogInformation("Handled request: {RequestName}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error handling request: {RequestName}", requestName);
                throw;
            }
        }
    }
}
