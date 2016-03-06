using System.Collections.Generic;

namespace Lazy.Authentication.Shared.Models
{
    public class RoleModel 
    {
        public string Name { get; set; }
        public List<string> Activities { get; set; }
    }
}