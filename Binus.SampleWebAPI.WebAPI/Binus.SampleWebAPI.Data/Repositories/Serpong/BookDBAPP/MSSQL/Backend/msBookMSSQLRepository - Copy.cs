using Binus.SampleWebAPI.Model.Base;
using Binus.SampleWebAPI.Data.Infrastructures.Serpong.MSSQL;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MSSQL.Backend;
using Binus.SampleWebAPI.Data.Infrastructures.Base.MSSQL;

namespace Binus.SampleWebAPI.Data.Repositories.Serpong.MSSQL.Backend
{
    public class msBookMSSQLRepository : BookDBMSSQLRepositoryBase<msBook>, ImsBookMSSQLRepository
    {
        public msBookMSSQLRepository(BookDBMSSQLIDBFactory DBFactory) : base(DBFactory)
        {
           
        }
    }

    public interface ImsBookMSSQLRepository : IMSSQLRepository<msBook>
    {
    }
}
