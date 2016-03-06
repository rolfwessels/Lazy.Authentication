using System.Linq;
using Lazy.Authentication.Api.Models.Mappers;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Shared.Interfaces.Shared;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Reference;

namespace Lazy.Authentication.Api.Common
{
    public class ProjectCommonController : BaseCommonController<Project, ProjectModel, ProjectReferenceModel, ProjectCreateUpdateModel>, IProjectControllerActions
    {
        
        public ProjectCommonController(IProjectManager projectManager)
            : base(projectManager)
        {
        }

    }
}