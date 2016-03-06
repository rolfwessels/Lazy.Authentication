using Lazy.Authentication.Dal.Models;

namespace Lazy.Authentication.Core.BusinessLogic.Components.Interfaces
{
    public interface IApplicationManager : IBaseManager<Application>
    {
        Application GetApplicationById(string clientId);
    }
}