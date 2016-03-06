using System.Collections.Generic;
using Lazy.Authentication.Dal.Models.Enums;

namespace Lazy.Authentication.Dal.Models
{
	public class Role
	{
		public string Name { get; set; }
		public List<Activity> Activities { get; set; }
	}
}