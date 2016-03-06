using System;
using Lazy.Authentication.Core.BusinessLogic.Components;
using Lazy.Authentication.Core.Vendor;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Persistance;

namespace Lazy.Authentication.Api.Database
{
    public class GeneralUnitOfWorkFactory   : IGeneralUnitOfWorkFactory
    {
        private readonly static Lazy<IGeneralUnitOfWork> _lazy;

        static GeneralUnitOfWorkFactory()
        {
            _lazy = new Lazy<IGeneralUnitOfWork>(OnValueFactory );
        }

        static IGeneralUnitOfWork OnValueFactory()
        {
            var memoryGeneralUnitOfWork = new MemoryGeneralUnitOfWork();
            memoryGeneralUnitOfWork.Applications.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "Lazy.Authentication.Api" });
            memoryGeneralUnitOfWork.Applications.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "Lazy.Authentication.Console" });
            memoryGeneralUnitOfWork.Applications.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "Lazy.Authentication.App" });

            var admin = new User() { Name = "Admin user", Email = "admin", HashedPassword = PasswordHash.CreateHash("admin!") };
            admin.Roles.Add(RoleManager.Admin.Name);
            memoryGeneralUnitOfWork.Users.Add(admin);

            var guest = new User() { Name = "Guest", Email = "guest@guest.com", HashedPassword = PasswordHash.CreateHash("guest!") };
            guest.Roles.Add(RoleManager.Guest.Name);
            memoryGeneralUnitOfWork.Users.Add(guest);
            return memoryGeneralUnitOfWork;
        }


        #region Implementation of IGeneralUnitOfWorkFactory

        public IGeneralUnitOfWork GetConnection()
        {
            return _lazy.Value;
        }

        #endregion
    }
}