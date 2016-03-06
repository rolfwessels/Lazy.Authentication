using FluentValidation;

namespace Lazy.Authentication.Dal.Validation
{
    public static class ValidationHelper
    {
        public static IRuleBuilder<T, string> MediumString<T>(this IRuleBuilder<T, string> options)
        {
            options.Length(1, 150);
            return options;
        }
    }
}