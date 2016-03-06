using System.Collections.Generic;
using System.Threading.Tasks;
using Lazy.Authentication.Dal.Models;

namespace Lazy.Authentication.Core.BusinessLogic.Components.Interfaces
{
    public interface IRoleManager 
    {
        Task<Role> GetRoleByName(string name);
        Task<List<Role>> Get();
    }
}