using Binus.SampleWebAPI.Data.DBContext.Common.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Base;


namespace Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL
{
    public class BinusWebAPIDBFactory : Disposable, BinusWebAPIIDBFactory
    {
        BinusWebAPIDBContext DBContext;


        public BinusWebAPIDBContext Init()
        {
            return DBContext ?? (DBContext = new BinusWebAPIDBContext());
        }



        protected override void DisposeCore()
        {
            if (DBContext != null)
                DBContext.Dispose();
        }
    }
}
