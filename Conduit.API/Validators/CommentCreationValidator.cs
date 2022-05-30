using Conduit.Data.Models;
using FluentValidation;

namespace Conduit.API.Validators;

public class CommentCreationValidator : AbstractValidator<CommentForCreationDto>
{
    public CommentCreationValidator()
    {
        RuleFor(x => x.Body).NotNull();
    }
}