using System.Threading.Tasks;
using Lazy.Authentication.Dal.Models;

namespace Lazy.Authentication.Core.BusinessLogic.Components.Interfaces
{
    public interface IUserManager : IBaseManager<User>
    {
        Task<User> Save(User user, string password);
        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<User> GetUserByEmail(string email);
        Task UpdateLastLoginDate(string email);
    }
}