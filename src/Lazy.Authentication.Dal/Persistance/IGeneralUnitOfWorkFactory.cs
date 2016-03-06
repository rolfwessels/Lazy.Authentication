using System.Security.Cryptography.X509Certificates;

namespace Lazy.Authentication.Dal.Persistance
{
    public interface IGeneralUnitOfWorkFactory
    {
        IGeneralUnitOfWork GetConnection();
    }
}