using FluentValidation;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Constraints;

namespace TeamHub.BLL.DtoValidators;

public class CommentRequestDtoValidator : AbstractValidator<CommentRequestDto>
{
    public CommentRequestDtoValidator()
    {
        var maxContentLength = CommentConstraints.maxContentLength;
        RuleFor(dto => dto.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(maxContentLength)
            .WithMessage($"Content cannot exceed {maxContentLength} characters.");
    }
}
