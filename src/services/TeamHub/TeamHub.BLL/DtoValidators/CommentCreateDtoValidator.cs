using FluentValidation;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Constraints;

namespace TeamHub.BLL.DtoValidators;

public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
{
    public CommentCreateDtoValidator()
    {
        RuleFor(dto => dto.TasksId).NotEmpty().WithMessage("Task ID is required.");

        var maxContentLength = CommentConstraints.maxContentLength;
        RuleFor(dto => dto.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(maxContentLength)
            .WithMessage($"Content cannot exceed {maxContentLength} characters.");
    }
}
