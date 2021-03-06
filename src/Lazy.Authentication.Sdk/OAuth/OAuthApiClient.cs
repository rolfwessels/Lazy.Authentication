using System;
using System.Net;
using System.Threading.Tasks;
using Lazy.Authentication.Sdk.Helpers;
using Lazy.Authentication.Sdk.RestApi;
using Lazy.Authentication.Shared.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Lazy.Authentication.Sdk.OAuth
{
    public class OAuthApiClient : OAuthApiClientBase
    {
        public OAuthApiClient(RestConnectionFactory restConnectionFactory) : base(restConnectionFactory)
        {
        }

        public async Task<TokenResponseModel> GenerateToken(TokenRequestModel tokenRequestModel)
        {
            var request = new RestRequest("Token", Method.POST);
            request.AddParameter("username", tokenRequestModel.UserName);
            request.AddParameter("password", tokenRequestModel.Password);
            request.AddParameter("grant_type", tokenRequestModel.GrantType);
            request.AddParameter("client_id", tokenRequestModel.ClientId);

            IRestResponse<TokenResponseModel> result =
                await _restClient.ExecuteAsyncWithLogging<TokenResponseModel>(request);
            ValidateResponse(result);
            var bearerToken = string.Format("{0} {1}", result.Data.TokenType, result.Data.AccessToken);
            _restClient.DefaultParameters.Add(new Parameter() { Type = ParameterType.HttpHeader, Name = "Authorization", Value = bearerToken });
            return result.Data;
        }

        #region Private Methods

        #endregion

        #region Nested type: ErrorResponse

        public class ErrorResponse
        {
            public string Error { get; set; }
            public string Error_description { get; set; }
        }

        #endregion

        #region Overrides of OAuthApiClientBase

        protected override void ValidateResponse<T>(IRestResponse<T> result)
        {
            if (result.StatusCode != HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(result.Content)) throw new Exception(string.Format("Api response for {0} is empty.", result.ResponseUri));
                var errorMessage = JsonConvert.DeserializeObject<ErrorResponse>(result.Content);
                throw new RestClientException(errorMessage.Error_description, result);
            }
        }

        #endregion
    }
}