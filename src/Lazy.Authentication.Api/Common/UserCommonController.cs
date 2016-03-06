using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using Lazy.Authentication.Api.Models.Mappers;
using Lazy.Authentication.Core.BusinessLogic.Components;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Shared.Interfaces.Shared;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Reference;
using log4net;

namespace Lazy.Authentication.Api.Common
{
    public class UserCommonController : BaseCommonController<User, UserModel, UserReferenceModel, UserCreateUpdateModel>,
                                        IUserControllerActions
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public UserCommonController(IUserManager userManager, IRoleManager roleManager) : base(userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region IUserControllerActions Members

        public async Task<UserModel> Register(RegisterModel model)
        {
            User user = model.ToDal();
            user.Roles.Add(RoleManager.Guest.Name);
            User savedUser = await _userManager.Save(user, model.Password);
            return savedUser.ToModel();
        }

        public Task<bool> ForgotPassword(string email)
        {
            return Task.Run(() =>
                {

                    _log.Warn(string.Format("User has called forgot password. We should send him and email to [{0}].",
                                            email));
                    return true;
                    
                });
        }

        #endregion

        #region Overrides of BaseCommonController<User,UserModel,UserReferenceModel,UserCreateUpdateModel>

        protected override async Task<User> AddAdditionalMappings(UserCreateUpdateModel model, User dal)
        {
            var addAdditionalMappings = await base.AddAdditionalMappings(model, dal);
            if (!addAdditionalMappings.Roles.Any()) addAdditionalMappings.Roles.Add(RoleManager.Guest.Name);
            return addAdditionalMappings;
        }

        #endregion

        public async Task<List<RoleModel>> Roles()
        {
            var roles = await _roleManager.Get();
            return roles.ToModels().ToList();
        }

        public async Task<UserModel> WhoAmI()
        {
            var userByEmail = await _userManager.GetUserByEmail(Thread.CurrentPrincipal.Identity.Name);
            return userByEmail.ToModel();
        }
    }
}