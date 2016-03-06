namespace Lazy.Authentication.Core.Mappers
{
    public static partial class MapCore
    {
        static MapCore()
        {
            CreateProjectMap();
            CreateOAuthMap();
            CreateUserMap();
        }

    }
}