using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lazy.Authentication.Api.WebApi.ODataSupport;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;

namespace Lazy.Authentication.Api.Common
{
    public abstract class ReadOnlyCommonControllerBase<TDal, TModel, TReferenceModel>
    {
        protected IBaseManager<TDal> _projectManager;

        public Task<IEnumerable<TReferenceModel>> Get(string query = null)
        {
            
            var queryToODataWrapper = new QueryToODataWrapper<TDal, TReferenceModel>(_projectManager.Query(), query, ToReferenceModelList);
            return Task.FromResult(queryToODataWrapper as IEnumerable<TReferenceModel>);
        }

      

        public Task<IEnumerable<TModel>> GetDetail(string query = null)
        {
            
            var queryToODataWrapper = new QueryToODataWrapper<TDal, TModel>(_projectManager.Query(), query, ToModelList);
            return Task.FromResult( queryToODataWrapper as IEnumerable<TModel>);
        }

        public async Task<TModel> Get(Guid id)
        {
            var task = await _projectManager.Get(id);
            return ToModel(task);
        }

        protected virtual TModel ToModel(TDal arg)
        {
            return Mapper.Map<TDal, TModel>(arg);
        }

        protected virtual IEnumerable<TModel> ToModelList(IEnumerable<TDal> arg)
        {
            return Mapper.Map<IEnumerable<TDal>, IEnumerable<TModel>>(arg);
        }

        protected virtual IEnumerable<TReferenceModel> ToReferenceModelList(IEnumerable<TDal> arg)
        {
            return Mapper.Map<IEnumerable<TDal>, IEnumerable<TReferenceModel>>(arg);
        }
    }
}