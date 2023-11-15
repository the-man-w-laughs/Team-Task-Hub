using FluentValidation.Results;

namespace Shared.Exceptions;

public class CustomValidationException : Exception
{
    public CustomValidationException() { }

    public CustomValidationException(string message)
        : base(message) { }

    public CustomValidationException(IEnumerable<ValidationFailure> errors)
        : base(BuildErrorMessage(errors)) { }

    private static string BuildErrorMessage(IEnumerable<ValidationFailure> errors)
    {
        var arr = errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}");
        return "Validation failed: " + string.Join(string.Empty, arr);
    }
}
