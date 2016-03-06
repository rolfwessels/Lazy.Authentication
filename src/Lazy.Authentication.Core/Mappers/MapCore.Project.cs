using AutoMapper;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Models.Reference;

namespace Lazy.Authentication.Core.Mappers
{
    public static partial class MapCore
	{
        public static void CreateProjectMap()
        {
            Mapper.CreateMap<Project, ProjectReference>();
        }

        public static ProjectReference ToReference(this Project project, ProjectReference projectReference = null)
        {
            return Mapper.Map(project, projectReference);
        }
	}
}