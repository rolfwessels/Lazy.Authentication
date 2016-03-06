using System;

namespace Lazy.Authentication.Dal.Models.Interfaces
{
	public interface IBaseDalModel
	{
		DateTime CreateDate { get; set; }
		DateTime UpdateDate { get; set; }
	}
}