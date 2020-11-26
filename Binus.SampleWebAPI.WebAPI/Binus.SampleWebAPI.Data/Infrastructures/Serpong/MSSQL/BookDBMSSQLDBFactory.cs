using Binus.SampleWebAPI.Data.DBContext.Serpong.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace  Binus.SampleWebAPI.Data.Infrastructures.Serpong.MSSQL
{
    public class BookDBMSSQLDBFactory: Disposable, BookDBMSSQLIDBFactory
    {
        BookDBMSSQLDBContext DBContext;


        public BookDBMSSQLDBContext Init()
        {
            return DBContext ?? (DBContext = new BookDBMSSQLDBContext());
        }



        protected override void DisposeCore()
        {
            if (DBContext != null)
                DBContext.Dispose();
        }
    }
}
