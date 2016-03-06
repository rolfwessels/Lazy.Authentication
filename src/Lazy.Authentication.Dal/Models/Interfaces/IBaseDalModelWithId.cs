using System;

namespace Lazy.Authentication.Dal.Models.Interfaces
{
	public interface IBaseDalModelWithId : IBaseDalModel
	{
		Guid Id { get; set; }
	}
}