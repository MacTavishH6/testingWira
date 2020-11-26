using Binus.SampleWebAPI.Data.Infrastructures.Base.MongoDB;
using Binus.SampleWebAPI.Data.Infrastructures.Serpong.MongoDB;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MongoDB.Backend;

namespace Binus.SampleWebAPI.Data.Repositories.Serpong.MongoDB.Backend
{
    public class msBookMongoDBRepository : BookDBMongoDBRepositoryBase<msBook>, ImsBookMongoDBRepository
    {
        public msBookMongoDBRepository(BookDBMongoDBIDBFactory DBFactory) : base(DBFactory)
        {
            this.CollectionNameData("msBook");
        }
    }

    public interface ImsBookMongoDBRepository : IMongoDBRepository<msBook>
    {

    }
}
