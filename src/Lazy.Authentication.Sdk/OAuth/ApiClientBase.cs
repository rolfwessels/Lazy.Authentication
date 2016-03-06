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
    public  abstract class ApiClientBase
    {
        protected RestClient _restClient;
        protected string _apiPrefix;

        protected ApiClientBase(RestConnectionFactory restConnectionFactory, string userController)
        {
            _restClient = restConnectionFactory.GetClient();
            _apiPrefix = userController;
        }

        protected virtual void ValidateResponse<T>(IRestResponse<T> result)
        {
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = JsonConvert.DeserializeObject<ErrorMessage>(result.Content);
                throw new Exception(errorMessage.Message);
            }
        }

        protected async Task<T> ExecuteAndValidate<T>(RestRequest request) where T : new()
        {
            var response = await _restClient.ExecuteAsyncWithLogging<T>(request);
            ValidateResponse(response);
            return response.Data;
        }

        protected async Task<bool> ExecuteAndValidateBool(RestRequest request)
        {
            var response = await _restClient.ExecuteAsyncWithLogging<bool>(request);
            ValidateResponse(response);
            return Convert.ToBoolean(response.Content);
        }

        protected RestRequest DefaultRequest(string projectController, Method get)
        {
            return new RestRequest(projectController, get) { RequestFormat = DataFormat.Json };
        }

     
    }
}