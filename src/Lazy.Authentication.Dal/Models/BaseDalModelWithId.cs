using System;
using Lazy.Authentication.Dal.Models.Interfaces;

namespace Lazy.Authentication.Dal.Models
{
	public abstract class BaseDalModelWithId : BaseDalModel, IBaseDalModelWithId
	{
		public BaseDalModelWithId()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
	}
}