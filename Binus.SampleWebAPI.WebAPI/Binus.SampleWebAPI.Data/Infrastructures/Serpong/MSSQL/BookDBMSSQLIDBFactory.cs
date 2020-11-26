using Binus.SampleWebAPI.Data.DBContext.Serpong.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Binus.SampleWebAPI.Data.Infrastructures.Serpong.MSSQL
{
    public interface BookDBMSSQLIDBFactory : IDisposable
    {
        BookDBMSSQLDBContext Init();
    }
}
