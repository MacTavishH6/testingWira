using Binus.SampleWebAPI.Data.DBContext.Common.MSSQL;
using System;

namespace Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL
{
    public interface BinusianIDNewIDBFactory : IDisposable
    {
        BinusianIDNewDBContext Init();
    }
}
