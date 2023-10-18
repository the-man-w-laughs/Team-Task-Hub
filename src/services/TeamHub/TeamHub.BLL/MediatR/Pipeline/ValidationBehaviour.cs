using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.Pipeline;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        foreach (var validator in _validators)
        {
            validator.ValidateAndThrowCustomException(request);
        }
        return await next();
    }
}
