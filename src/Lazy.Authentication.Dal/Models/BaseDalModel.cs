using System;
using Lazy.Authentication.Dal.Models.Interfaces;

namespace Lazy.Authentication.Dal.Models
{
	public abstract class BaseDalModel : IBaseDalModel
	{
		public BaseDalModel()
		{
			CreateDate = DateTime.Now;
			UpdateDate = DateTime.Now;
		}

		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
	}
}