using System;
using MongoDB.Driver;
using System.Configuration;


namespace Binus.SampleWebAPI.Data.DBContext.Serpong.MongoDB
{
    public class BookDBMongoDBDBContext : IDisposable
    {
        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;
        static BookDBMongoDBDBContext() {
             _client = new MongoClient(ConfigurationManager.AppSettings["BookDBMongoDBContext"].ToString());
             _database = _client.GetDatabase("BookDB");
            //var Ebooks = database.GetCollection<Ebooks>("msEbooks");
            //var roles = database.GetCollection<IdentityRole>("roles");
            
        }
       
       

        public IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name.ToLower() + "s");
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string CollectionNameData)
        {
           
            return _database.GetCollection<TEntity>(CollectionNameData);
        }

        public void Dispose()
        {
        }

      
    }


}
