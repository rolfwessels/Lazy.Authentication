using Lazy.Authentication.Sdk.RestApi.Base;
using Lazy.Authentication.Shared;
using Lazy.Authentication.Shared.Interfaces.Shared;
using Lazy.Authentication.Shared.Models;
using Lazy.Authentication.Shared.Models.Reference;

namespace Lazy.Authentication.Sdk.RestApi
{
    public class ProjectApiClient : BaseCrudApiClient<ProjectModel, ProjectCreateUpdateModel, ProjectReferenceModel>,
        IProjectControllerActions
    {
        public ProjectApiClient(RestConnectionFactory restConnectionFactory)
            : base(restConnectionFactory, RouteHelper.ProjectController)
        {
        }

    }
}
