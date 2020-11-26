using Binus.SampleWebAPI.Data.DBContext.Serpong.Oracle;
using Binus.SampleWebAPI.Data.Infrastructures.Base;

namespace Binus.SampleWebAPI.Data.Infrastructures.Serpong.Oracle
{
    public class BookDBOracleDBFactory : Disposable, BookDBOracleIDBFactory
    {
        BookDBOracleDBContext DBContext;


        public BookDBOracleDBContext Init()
        {
            return DBContext ?? (DBContext = new BookDBOracleDBContext());
        }



        protected override void DisposeCore()
        {
            if (DBContext != null)
                DBContext.Dispose();
        }
    }
}
