using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using FluentValidation;

namespace Conduit.API.Validators;

public class UserEditingValidator : AbstractValidator<UserForUpdatingDto>
{
    public UserEditingValidator()
    {
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotNull();
    }
}