using Binus.SampleWebAPI.Data.DBContext.Serpong.Oracle;
using System;

namespace Binus.SampleWebAPI.Data.Infrastructures.Serpong.Oracle
{
    public interface BookDBOracleIDBFactory : IDisposable
    {
        BookDBOracleDBContext Init();
    }
}
