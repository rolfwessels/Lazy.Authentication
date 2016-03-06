using System.Collections.Generic;
using System.Threading.Tasks;
using Lazy.Authentication.Shared.Interfaces.Base;
using Lazy.Authentication.Shared.Models;

namespace Lazy.Authentication.Shared.Interfaces.Shared
{
    public interface IUserControllerActions : ICrudController<UserModel, UserCreateUpdateModel>
	{
	    Task<UserModel> Register(RegisterModel user);
	    Task<bool> ForgotPassword(string email);
        Task<UserModel> WhoAmI();
        Task<List<RoleModel>> Roles();
	}
}