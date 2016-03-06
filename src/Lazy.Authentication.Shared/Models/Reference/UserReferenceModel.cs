using Lazy.Authentication.Shared.Models.Base;

namespace Lazy.Authentication.Shared.Models.Reference
{
    public class UserReferenceModel : BaseReferenceModelWithName
    {
        public string Email { get; set; }
    }
}