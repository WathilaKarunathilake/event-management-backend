// <copyright file="ValidationBehavior{TRequest,TResponse}.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Behaviours
{
    using EventManagementAPI.Core.Application.Response;
    using EventManagementAPI.Core.Domain.Errors;
    using FluentValidation;
    using MediatR;

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (this.validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(this.validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var firstFailure = validationResults
                    .SelectMany(r => r.Errors)
                    .FirstOrDefault(f => f != null);

                if (firstFailure != null)
                {
                    var resultType = typeof(TResponse);
                    if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
                    {
                        var error = Error.Validation("ValidationError", firstFailure.ErrorMessage);

                        var failureResult = typeof(Result<>)
                            .MakeGenericType(resultType.GetGenericArguments())
                            .GetMethod("Failure") !
                            .Invoke(null, new object[] { error });

                        return (TResponse)failureResult!;
                    }

                    throw new ValidationException(new[] { firstFailure });
                }
            }

            return await next();
        }
    }
}
