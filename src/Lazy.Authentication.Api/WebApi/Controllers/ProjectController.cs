using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Lazy.Authentication.Api.Common;
using Lazy.Authentication.Api.WebApi.Attributes;
using Lazy.Authentication.Api.WebApi.ODataSupport;
using Lazy.Authentication.Dal.Models.Enums;
using Lazy.Authentication.Shared;
using Lazy.Authentication.Shared.Interfaces.Base;
using Lazy.Authentication.Shared.Interfaces.Shared;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Reference;

namespace Lazy.Authentication.Api.WebApi.Controllers
{

    /// <summary>
	///     Api controller for managing all the project
	/// </summary>
    [RoutePrefix(RouteHelper.ProjectController)]
    public class ProjectController : ApiController, IProjectControllerActions, IBaseControllerLookups<ProjectModel, ProjectReferenceModel>
    {
	    private readonly ProjectCommonController _projectCommonController;
	    
        public ProjectController(ProjectCommonController projectCommonController)
        {
            _projectCommonController = projectCommonController;
        }

        /// <summary>
        ///     Returns list of all the projects as references
        /// </summary>
        /// <returns>
        /// </returns>
        [Route,AuthorizeActivity(Activity.ReadProject) , QueryToODataFilter]
        public Task<IEnumerable<ProjectReferenceModel>> Get()
        {   
            return _projectCommonController.Get(Request.GetQuery());
        }

        /// <summary>
        /// GetCounter all projects with their detail.
        /// </summary>
        /// <returns></returns>
        [Route(RouteHelper.WithDetail),AuthorizeActivity(Activity.ReadProject), QueryToODataFilter]
        public Task<IEnumerable<ProjectModel>> GetDetail()
		{
		    return _projectCommonController.GetDetail(Request.GetQuery());
		}


        /// <summary>
		///     Returns a project by his Id.
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.ReadProject)]
		public Task<ProjectModel> Get(Guid id)
		{
            return _projectCommonController.Get(id);
		}

	    /// <summary>
	    ///     Updates an instance of the project item.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <param name="model">The project.</param>
	    /// <returns>
	    /// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.UpdateProject) , HttpPut]
		public Task<ProjectModel> Update(Guid id, ProjectCreateUpdateModel model)
		{
            return _projectCommonController.Update(id, model);
		}

	    /// <summary>
	    ///     Add a new project
	    /// </summary>
	    /// <param name="model">The project.</param>
	    /// <returns>
	    /// </returns>
        [Route, AuthorizeActivity(Activity.InsertProject), HttpPost]
		public Task<ProjectModel> Insert(ProjectCreateUpdateModel model)
		{
            return _projectCommonController.Insert(model);
		}

	    /// <summary>
	    ///     Deletes the specified project.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <returns>
	    /// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.DeleteProject)]
		public Task<bool> Delete(Guid id)
		{
            return _projectCommonController.Delete(id);
		}

		
	}
}