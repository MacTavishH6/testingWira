using Binus.SampleWebAPI.Data.DBContext.Common.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Base;

namespace Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL
{
    public class HRISDBDBFactory : Disposable, HRISDBIDBFactory
    {
        HRISDBDBContext DBContext;


        public HRISDBDBContext Init()
        {
            return DBContext ?? (DBContext = new HRISDBDBContext());
        }



        protected override void DisposeCore()
        {
            if (DBContext != null)
                DBContext.Dispose();
        }
    }
}
