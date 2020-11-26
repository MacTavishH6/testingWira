using Binus.SampleWebAPI.Data.DBContext.Serpong.MongoDB;
using Binus.SampleWebAPI.Data.Infrastructures.Base;


namespace Binus.SampleWebAPI.Data.Infrastructures.Serpong.MongoDB
{
    public class BookDBMongoDBDBFactory : Disposable, BookDBMongoDBIDBFactory
    {
        BookDBMongoDBDBContext DBContext;


        public BookDBMongoDBDBContext Init()
        {
            return DBContext ?? (DBContext = new BookDBMongoDBDBContext());
        }



        protected override void DisposeCore()
        {
            if (DBContext != null)
                DBContext.Dispose();
        }
    }
}
