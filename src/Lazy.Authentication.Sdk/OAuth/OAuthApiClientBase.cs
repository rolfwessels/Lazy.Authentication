using System.Net;
using Lazy.Authentication.Sdk.RestApi;
using Lazy.Authentication.Shared;
using Newtonsoft.Json;
using RestSharp;

namespace Lazy.Authentication.Sdk.OAuth
{
    public  abstract class OAuthApiClientBase : ApiClientBase
    {
        protected OAuthApiClientBase(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory, RouteHelper.UserController)
        {
        }

        protected override void ValidateResponse<T>(IRestResponse<T> result)
        {
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = JsonConvert.DeserializeObject<OAuthApiClient.ErrorResponse>(result.Content);
                throw new RestClientException(errorMessage.Error_description, result);
            }
        }

        
    }
}