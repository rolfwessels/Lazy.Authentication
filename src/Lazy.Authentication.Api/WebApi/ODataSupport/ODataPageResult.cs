using System.Collections.Generic;

namespace Lazy.Authentication.Api.WebApi.ODataSupport
{
    public class ODataPageResult<T>
    {
        public List<T> Items { get; set; }

        public long? Count { get; set; }
    }
}