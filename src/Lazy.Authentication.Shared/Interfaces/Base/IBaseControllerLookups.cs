using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lazy.Authentication.Shared.Interfaces.Base
{
    public interface IBaseControllerLookups<TDetails, TModelReference>
    {
        Task<IEnumerable<TModelReference>> Get();
        Task<IEnumerable<TDetails>> GetDetail();
    }
}