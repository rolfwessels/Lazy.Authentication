using System;
using RestSharp;

namespace Lazy.Authentication.Sdk.OAuth
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
    public class RestClientException : Exception
    {
        private readonly IRestResponse _response;

        public RestClientException(string message, IRestResponse response) : base(message)
        {
            _response = response;
        }

        public IRestResponse Response
        {
            get { return _response; }
        }
    }
}