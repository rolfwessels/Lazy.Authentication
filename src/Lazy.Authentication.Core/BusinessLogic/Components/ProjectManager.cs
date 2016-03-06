using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Persistance;

namespace Lazy.Authentication.Core.BusinessLogic.Components
{
    public class ProjectManager : BaseManager<Project>, IProjectManager
    {
        public ProjectManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
        }

        #region Overrides of BaseManager<Project>

        protected override IRepository<Project> Repository
        {
            get { return _generalUnitOfWork.Projects; }
        }

        #endregion
    }
}