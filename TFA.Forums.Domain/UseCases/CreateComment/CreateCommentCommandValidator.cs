using FluentValidation;
using TFA.Forums.Domain.Exceptions;

namespace TFA.Forums.Domain.UseCases.CreateComment;

internal class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(c => c.TopicId)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(c => c.Text)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
    }
}