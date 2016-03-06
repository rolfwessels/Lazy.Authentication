using System.Linq;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Persistance;

namespace Lazy.Authentication.Core.BusinessLogic.Components
{
    public class ApplicationManager : BaseManager<Application>, IApplicationManager
    {
        public ApplicationManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
        }

        protected override IRepository<Application> Repository
        {
            get { return _generalUnitOfWork.Applications; }
        }

        #region IApplicationManager Members

        public Application GetApplicationById(string clientId)
        {
            return _generalUnitOfWork.Applications.FindOne(x => x.ClientId == clientId).Result;
        }

        #endregion
    }
}