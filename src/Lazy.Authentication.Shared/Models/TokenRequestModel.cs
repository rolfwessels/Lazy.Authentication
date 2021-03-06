using RestSharp.Serializers;

namespace Lazy.Authentication.Shared.Models
{
    public class TokenRequestModel
    {
        public TokenRequestModel()
        {
            GrantType = "password";
        }

        [SerializeAs(Name = "username")]
        public string UserName { get; set; }

        [SerializeAs(Name = "password")]
        public string Password { get; set; }

        [SerializeAs(Name = "client_id")]
        public string ClientId { get; set; }

        [SerializeAs(Name = "grant_type")]
        public string GrantType { get; set; }
    }
}