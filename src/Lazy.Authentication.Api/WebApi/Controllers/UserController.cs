using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Lazy.Authentication.Api.Common;
using Lazy.Authentication.Api.WebApi.Attributes;
using Lazy.Authentication.Api.WebApi.ODataSupport;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Models.Enums;
using Lazy.Authentication.Shared;
using Lazy.Authentication.Shared.Interfaces.Base;
using Lazy.Authentication.Shared.Interfaces.Shared;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Reference;

namespace Lazy.Authentication.Api.WebApi.Controllers
{
    /// <summary>
	///     Api controller for managing all the user
	/// </summary>
    [RoutePrefix(RouteHelper.UserController)]
    public class UserController : ApiController, IUserControllerActions, IBaseControllerLookups<UserModel, UserReferenceModel>
    {
	    private readonly UserCommonController _userCommonController;
	    
        public UserController(UserCommonController userCommonController)
        {
            _userCommonController = userCommonController;
        }

        

        /// <summary>
        ///     Returns list of all the users as references
        /// </summary>
        /// <returns>
        /// </returns>
        [Route,AuthorizeActivity(Activity.ReadUsers) , QueryToODataFilter]
        public Task<IEnumerable<UserReferenceModel>> Get()
        {   
            return _userCommonController.Get(Request.GetQuery());
        }

        /// <summary>
        /// GetCounter all users with their detail.
        /// </summary>
        /// <returns></returns>
        [Route(RouteHelper.WithDetail),AuthorizeActivity(Activity.ReadUsers), QueryToODataFilter]
        public Task<IEnumerable<UserModel>> GetDetail()
		{
		    return _userCommonController.GetDetail(Request.GetQuery());
		}


        /// <summary>
		///     Returns a user by his Id.
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.ReadUsers)]
		public Task<UserModel> Get(Guid id)
		{
            return _userCommonController.Get(id);
		}

	    /// <summary>
	    ///     Updates an instance of the user item.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <param name="model">The user.</param>
	    /// <returns>
	    /// </returns>
        [Route(RouteHelper.WithId), AuthorizeActivity(Activity.UpdateUsers), HttpPut]
		public Task<UserModel> Update(Guid id, UserCreateUpdateModel model)
		{
            return _userCommonController.Update(id, model);
		}

	    /// <summary>
	    ///     Add a new user
	    /// </summary>
	    /// <param name="model">The user.</param>
	    /// <returns>
	    /// </returns>
        [Route, AuthorizeActivity(Activity.InsertUsers), HttpPost]
		public Task<UserModel> Insert(UserCreateUpdateModel model)
		{
            return _userCommonController.Insert(model);
		}

	    /// <summary>
	    ///     Deletes the specified user.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <returns>
	    /// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.DeleteUser)]
		public Task<bool> Delete(Guid id)
		{
            return _userCommonController.Delete(id);
		}

        
        [Route(RouteHelper.UserControllerRoles), AuthorizeActivity(Activity.ReadUsers), HttpGet]
        public Task<List<RoleModel>> Roles()
        {
            return _userCommonController.Roles();
        }

		#region Other actions

	    /// <summary>
	    /// Registers the specified user.
	    /// </summary>
	    /// <param name="user">The user.</param>
	    /// <returns></returns>
        [Route(RouteHelper.UserControllerRegister), AllowAnonymous, HttpPost]
		public Task<UserModel> Register(RegisterModel user)
		{
            return _userCommonController.Register(user);
		}


	    /// <summary>
	    /// Forgot the password sends user an email with his password.
	    /// </summary>
	    /// <param name="email">The email.</param>
	    /// <returns></returns>
        [Route(RouteHelper.UserControllerForgotPassword), AllowAnonymous, HttpGet]
		public Task<bool> ForgotPassword(string email)
		{
            return _userCommonController.ForgotPassword(email);
		}

        /// <summary>
        ///     Return the current user.
        /// </summary>
        /// <returns>
        /// </returns>
        [Route(RouteHelper.UserControllerWhoAmI), AuthorizeActivity, HttpGet]
        public Task<UserModel> WhoAmI()
        {
            return _userCommonController.WhoAmI();
        }


		#endregion
	}
}