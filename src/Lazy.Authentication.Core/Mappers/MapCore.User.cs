using AutoMapper;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Models.Reference;

namespace Lazy.Authentication.Core.Mappers
{
    public static partial class MapCore
	{
        public static void CreateUserMap()
        {
            Mapper.CreateMap<User, UserReference>();
        }

        public static UserReference ToReference(this User user, UserReference userReference = null)
        {
            return Mapper.Map(user, userReference);
        }
	}
}