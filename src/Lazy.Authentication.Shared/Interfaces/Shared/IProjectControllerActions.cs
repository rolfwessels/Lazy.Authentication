using System.Threading.Tasks;
using Lazy.Authentication.Shared.Interfaces.Base;
using Lazy.Authentication.Shared.Models;

namespace Lazy.Authentication.Shared.Interfaces.Shared
{
    public interface IProjectControllerActions : ICrudController<ProjectModel, ProjectCreateUpdateModel>
	{
	}
}