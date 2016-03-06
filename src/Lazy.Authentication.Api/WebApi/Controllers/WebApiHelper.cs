using System;
using System.Net.Http;

namespace Lazy.Authentication.Api.WebApi.Controllers
{
    public static class WebApiHelper
    {
        public static string GetQuery(this HttpRequestMessage request)
        {
            var query = new Uri(request.RequestUri.AbsoluteUri);
            return query.Query;
        }
    }
}