using Binus.SampleWebAPI.Data.Infrastructures.Base.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL;
using Binus.WebAPI.Model.Common;

namespace Binus.SampleWebAPI.Data.Repositories.Common.MSSQL
{
    public class UserHRISDBRepository : HRISDBRepositoryBase<UserHRISDB>, IUserHRISDBRepository
    {
        public UserHRISDBRepository(HRISDBIDBFactory DBFactory) : base(DBFactory)
        {
         
        }


    }

    public interface IUserHRISDBRepository : IMSSQLRepository<UserHRISDB>
    {
       
    }
}
