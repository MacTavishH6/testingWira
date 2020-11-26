using Binus.SampleWebAPI.Data.Infrastructures.Base.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL;
using Binus.WebAPI.Model.Common;

namespace Binus.SampleWebAPI.Data.Repositories.Common.MSSQL
{
    public class ControllerRepository : BinusWebAPIRepositoryBase<WebAPIController>, IControllerRepository
    {
        public ControllerRepository(BinusWebAPIIDBFactory DBFactory) : base(DBFactory)
        {
           
        }
    }

    public interface IControllerRepository : IMSSQLRepository<WebAPIController>
    {
   
    }

}
