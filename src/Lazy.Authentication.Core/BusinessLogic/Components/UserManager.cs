﻿using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lazy.Authentication.Core.BusinessLogic.Components.Interfaces;
using Lazy.Authentication.Core.Vendor;
using Lazy.Authentication.Dal.Models;
using Lazy.Authentication.Dal.Persistance;
using log4net;
using Lazy.Authentication.Utilities.Helpers;

namespace Lazy.Authentication.Core.BusinessLogic.Components
{
    public class UserManager : BaseManager<User>, IUserManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserManager(BaseManagerArguments baseManagerArguments) : base(baseManagerArguments)
        {
        }

        #region Overrides of BaseManager<User>

        public override Task<User> Save(User entity)
        {
            return Save(entity, null);
        }
        
        protected override void DefaultModelNormalize(User user)
        {
            user.Email = (user.Email??"").ToLower();
            user.HashedPassword = user.HashedPassword == null || !user.HashedPassword.StartsWith("1000:")
                                      ? PasswordHash.CreateHash(user.HashedPassword ?? DateTime.Now.ToString(CultureInfo.InvariantCulture))
                                      : user.HashedPassword ;
        }

        protected override async Task Validate(User entity)
        {
            await base.Validate(entity);
            var missingRoles = entity.Roles.Where(x => RoleManager.GetRole(x) == null).ToArray();
            if (missingRoles.Any())
            {
                throw new ArgumentException(string.Format("The following role '{0}' does not exist.", missingRoles.StringJoin()));
            }
        }

        #endregion

        #region IUserManager Members

        public async Task<User> Save(User user, string password)
        {
            User found = await Get(user.Id);
            user.HashedPassword = password != null || found == null
                                      ? PasswordHash.CreateHash(password ??
                                                                user.HashedPassword ?? DateTime.Now.ToString(CultureInfo.InvariantCulture))
                                      : found.HashedPassword;
            if (found == null)
            {
                return await Insert(user);
            }
            await Update(user);
            return user;
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            User user = await GetUserByEmail(email);
            if (user != null && user.HashedPassword != null)
            {
                if (!PasswordHash.ValidatePassword(password, user.HashedPassword))
                {
                    user = null;
                    _log.Info(string.Format("Invalid password for user '{0}'", email));
                }
            }
            else
            {
                _log.Info(string.Format("Invalid user '{0}'", email));
            }
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _generalUnitOfWork.Users.FindOne(x => x.Email == email.ToLower());
        }

        public async Task UpdateLastLoginDate(string email)
        {
            User userFound = await GetUserByEmail(email);
            if (userFound == null) throw new ArgumentException("Invalid email address.");
            userFound.LastLoginDate = DateTime.Now;
            await  Update(userFound);
        }

        #endregion

        #region Overrides of BaseManager<User>

        protected override IRepository<User> Repository
        {
            get { return _generalUnitOfWork.Users; }
        }

        #endregion
    }
}