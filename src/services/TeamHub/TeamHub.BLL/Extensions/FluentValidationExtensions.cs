using FluentValidation;
using Shared.Exceptions;

namespace TeamHub.BLL.Extensions;

public static class FluentValidationExtensions
{
    public static void ValidateAndThrowCustomException<T>(this IValidator<T> validator, T instance)
    {
        var res = validator.Validate(instance);

        if (!res.IsValid)
        {
            throw new CustomValidationException(res.Errors);
        }
    }
}
