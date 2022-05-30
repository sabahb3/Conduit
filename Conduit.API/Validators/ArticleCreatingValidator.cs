using Conduit.Data.Models;
using FluentValidation;

namespace Conduit.API.Validators;

public class ArticleCreatingValidator :AbstractValidator<ArticleForCreation>
{
    public ArticleCreatingValidator()
    {
        RuleFor(x => x.Title).NotNull();
        RuleFor(x => x.Body).NotNull();
        RuleFor(x => x.Description).NotNull();
    }
}