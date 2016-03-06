using FluentValidation;
using FluentValidation.Results;
using Lazy.Authentication.Dal.Models;

namespace Lazy.Authentication.Dal.Validation
{
    public interface IValidatorFactory
    {
        ValidationResult For<T>(T user);
        void ValidateAndThrow<T>(T user);
        IValidator<T> Validator<T>();
    }

    
}