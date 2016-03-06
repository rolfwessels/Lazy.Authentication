using Lazy.Authentication.Dal.Models.Base;

namespace Lazy.Authentication.Dal.Models.Reference
{
    public class UserReference : BaseReferenceWithName
    {
        public string Email { get; set; }
    }
}