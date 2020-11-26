using Binus.SampleWebAPI.Data.DBContext.Serpong.MongoDB;
using System;

namespace Binus.SampleWebAPI.Data.Infrastructures.Serpong.MongoDB
{
    public interface BookDBMongoDBIDBFactory : IDisposable
    {
        BookDBMongoDBDBContext Init();
    }
}
