using Binus.SampleWebAPI.Data.Infrastructures.Base.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL;
using Binus.WebAPI.Model.Common;

namespace Binus.SampleWebAPI.Data.Repositories.Common.MSSQL
{
    public class BinusianPhotoRepository : BinusianIDNewRepositoryBase<BinusianPhoto>, IBinusianPhotoRepository
    {
        public BinusianPhotoRepository(BinusianIDNewIDBFactory DBFactory) : base(DBFactory)
        {
           
        }
    }

    public interface IBinusianPhotoRepository : IMSSQLRepository<BinusianPhoto>
    {
     
    }
}
