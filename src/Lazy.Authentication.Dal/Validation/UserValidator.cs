﻿using FluentValidation;
using Lazy.Authentication.Dal.Models;

namespace Lazy.Authentication.Dal.Validation
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1405:ComVisibleTypeBaseTypesShouldBeComVisible")]
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotNull().MediumString();
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.HashedPassword).NotEmpty();
            RuleFor(x => x.Roles).NotEmpty();
        }
    }
}