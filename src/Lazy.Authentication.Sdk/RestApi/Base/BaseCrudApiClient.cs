using System;
using System.Threading.Tasks;
using Lazy.Authentication.Shared;
using Lazy.Authentication.Shared.Interfaces.Base;
using Lazy.Authentication.Shared.Models.Interfaces;
using RestSharp;

namespace Lazy.Authentication.Sdk.RestApi.Base
{
    public class BaseCrudApiClient<TModel, TDetailModel, TReferenceModel> : BaseGetApiClient<TModel, TReferenceModel>,
                                                                            ICrudController<TModel, TDetailModel>
        where TModel : IBaseModel, new()
    {
        protected BaseCrudApiClient(RestConnectionFactory restConnectionFactory, string userController)
            : base(restConnectionFactory, userController)
        {
        }

        #region ICrudController<TModel,TDetailModel> Members

        public async Task<TModel> Get(Guid id)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidate<TModel>(request);
        }

        public async Task<TModel> Insert(TDetailModel model)
        {
            RestRequest request = DefaultRequest(_apiPrefix, Method.POST);
            request.AddBody(model);
            return await ExecuteAndValidate<TModel>(request);
        }

        public async Task<TModel> Update(Guid id, TDetailModel model)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.PUT);
            request.AddUrlSegment("id", id.ToString());
            request.AddBody(model);
            return await ExecuteAndValidate<TModel>(request);
        }

        public async Task<bool> Delete(Guid id)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.WithId), Method.DELETE);
            request.AddUrlSegment("id", id.ToString());
            return await ExecuteAndValidateBool(request);
        }

        #endregion
    }

    public static class StringHelper
    {
        public static string UriCombine(this string baseUri, params string[] addition)
        {
            string uri = baseUri;
            foreach (string moreUri in addition)
            {
                uri = EnsureEndsWith(uri, "/") + EnsureDoesNotStartWith(moreUri, "/");
            }
            return uri;
        }

        public static string EnsureEndsWith(this string value, string postFix)
        {
            if (value.EndsWith(postFix)) return value;
            return value + postFix;
        }

        public static string EnsureDoesNotStartWith(this string value, string prefix)
        {
            if (value.StartsWith(prefix))
            {
                return value.Substring(prefix.Length);
            }
            return value;
        }

    }
}