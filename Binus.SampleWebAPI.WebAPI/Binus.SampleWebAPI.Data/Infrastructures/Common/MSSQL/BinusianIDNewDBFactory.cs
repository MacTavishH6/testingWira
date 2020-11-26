using Binus.SampleWebAPI.Data.DBContext.Common.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Base;


namespace Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL
{
    public class BinusianIDNewDBFactory : Disposable, BinusianIDNewIDBFactory
    {
        BinusianIDNewDBContext DBContext;


        public BinusianIDNewDBContext Init()
        {
            return DBContext ?? (DBContext = new BinusianIDNewDBContext());
        }



        protected override void DisposeCore()
        {
            if (DBContext != null)
                DBContext.Dispose();
        }
    }
}
