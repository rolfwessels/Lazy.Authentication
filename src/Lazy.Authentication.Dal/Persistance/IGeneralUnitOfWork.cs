using System;
using Lazy.Authentication.Dal.Models;

namespace Lazy.Authentication.Dal.Persistance
{
	public interface IGeneralUnitOfWork
	{
		IRepository<User> Users { get;  }
		IRepository<Application> Applications { get; }
		IRepository<Project> Projects { get; }
	}
}