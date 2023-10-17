using FluentValidation;

namespace TeamHub.BLL.DtoValidators.Utils;

public class DateTimeValidator : AbstractValidator<DateTime?>
{
    public DateTimeValidator()
    {
        RuleFor(date => date).Must(BeAValidDate).WithMessage("The date must be in the future.");
    }

    private bool BeAValidDate(DateTime? date)
    {
        return date == null || date.Value > DateTime.Now;
    }
}
