using Binus.SampleWebAPI.Data.Infrastructures.Base.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL;
using Binus.WebAPI.Model.Common;

namespace Binus.SampleWebAPI.Data.Repositories.Common.MSSQL
{
    public class EmployeeRepository : BinusWebAPIRepositoryBase<EmployeeDB>, IEmployeeRepository
    {
        public EmployeeRepository(BinusWebAPIIDBFactory DBFactory) : base(DBFactory)
        {
           
        }
    }


    public interface IEmployeeRepository : IMSSQLRepository<EmployeeDB>
    {

    }

}
