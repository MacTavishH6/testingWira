using Binus.SampleWebAPI.Data.Infrastructures.Base.Oracle;
using Binus.SampleWebAPI.Data.Infrastructures.Serpong.Oracle;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.Oracle.Backend;

namespace Binus.SampleWebAPI.Data.Repositories.Serpong.Oracle.Backend
{
    public class msBookOracleRepository : BookDBOracleRepositoryBase<msBook>, ImsBookOracleRepository
    {
        
        public msBookOracleRepository(BookDBOracleIDBFactory DBFactory) : base(DBFactory) {
           
        }
    }

    public interface ImsBookOracleRepository : IOracleRepository<msBook>
    {

    }
}
