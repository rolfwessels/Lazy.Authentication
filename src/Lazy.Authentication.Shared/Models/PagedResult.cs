using System.Collections.Generic;

namespace Lazy.Authentication.Shared.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int Count { get; set; }
    }
}