using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Bson;
using Binus.SampleWebAPI.Data.DBContext.Serpong.MongoDB;
using Binus.SampleWebAPI.Model.Base;
using System.Configuration;
using System.Linq;
using Binus.SampleWebAPI.Model.Common;

namespace Binus.SampleWebAPI.Data.Infrastructures.Serpong.MongoDB
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(
            this IEnumerable<T> source, params T[] tail)
        {
            return source.Concat(tail);
        }
    }
    public abstract class BookDBMongoDBRepositoryBase<T> 
        where T : class

    {
        #region Properties
        private BookDBMongoDBDBContext DataContext;
        public string ConStr;
        public string CollectionName;
        private int PageSize;
        private int CurrentPage;
        private List<FieldDefinition<BsonDocument>[]> Lookup;
        private FieldDefinition<T> EntityFieldDefinition;
        private List<SortField> SortDefinition;
        //private List<String> SortDescDefinition;
        private List<BsonDocument> Unwind;
        private List<BsonDocument> Group;
        private List<BsonDocument> Sort;
        private List<BsonDocument> Match;
        private int Limit;

        enum ExecType { List, Single, NoExecRecord };
        protected BookDBMongoDBIDBFactory DBFactory
        {
            get;
            private set;
        }


        protected BookDBMongoDBDBContext DBContext
        {
            get { return DataContext ?? (DataContext = DBFactory.Init()); }
        }

        public void CollectionNameData(string CollectionName)
        {
            this.CollectionName = CollectionName;
        }

        protected BookDBMongoDBRepositoryBase(BookDBMongoDBIDBFactory DBFactory)
        {
          
            this.DBFactory = DBFactory;
            this.DataContext = this.DBContext;

        }
       public async Task SetPaging(int PageSize, int CurrentPage)
        {
            //CurrentPage * Page Size = Skip
            this.PageSize = PageSize;
            this.CurrentPage = CurrentPage;
            await Task.Delay(100);
        }

        public async Task SortData(List<SortField> Data)
        {
               SortDefinition = Data;
           // SortDescDefinition = DataDesc;
            await Task.Delay(100);
        }
        public async Task<GetOneResult<TEntity>> GetOne<TEntity>(string id) where TEntity : class, new()
        {
            var filter = Builders<TEntity>.Filter.Eq(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), id);
            return await GetOne<TEntity>(filter);
        }

        public async Task<GetOneResult<TEntity>> GetOne<TEntity>(string id, ProjectionDefinition<TEntity> Projection) where TEntity : class, new()
        {
            var filter = Builders<TEntity>.Filter.Eq(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), id);
            return await GetOne<TEntity>(filter, Projection);
        }

        /// <summary>
        /// A generic GetOne method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name=ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString()></param>
        /// <returns></returns>
        public async Task<GetOneResult<TEntity>> GetOne<TEntity>(FilterDefinition<TEntity> filter) where TEntity : class, new()
        {
            var res = new GetOneResult<TEntity>();
            
                var collection = GetCollection<TEntity>();
                var entity = await collection.Find(filter).SingleOrDefaultAsync();
                if (entity != null)
                {
                    res.Entity = entity;
                }
                res.Success = true;
                return res;
           
        }
        public async Task<GetOneResult<TEntity>> GetOne<TEntity>(FilterDefinition<TEntity> filter, ProjectionDefinition<TEntity> Projection) where TEntity : class, new()
        {
            var res = new GetOneResult<TEntity>();

            var collection = GetCollection<TEntity>();
            var entity = await collection.Find(filter).Project<TEntity>(Projection).SingleOrDefaultAsync();
            if (entity != null)
            {
                res.Entity = entity;
            }
            res.Success = true;
            return res;

        }

        /// <summary>
        /// A generic get many method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<GetManyResult<TEntity>> GetMany<TEntity>(IEnumerable<string> ids) where TEntity : class, new()
        {
            
                var collection = GetCollection<TEntity>();
                var filter = Builders<TEntity>.Filter.In(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ids);
                return await GetMany<TEntity>(filter);
          
        }
        public async Task<GetManyResult<TEntity>> GetMany<TEntity>(IEnumerable<ObjectId> ids) where TEntity : class, new()
        {

            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.In(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ids);
            return await GetMany<TEntity>(filter);

        }
        public async Task<GetManyResult<TEntity>> GetMany<TEntity>(IEnumerable<ObjectId> ids, ProjectionDefinition<TEntity> Projection) where TEntity : class, new()
        {

            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.In(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ids);
            return await GetMany<TEntity>(filter, Projection);

        }
        public async Task<GetManyResult<TEntity>> GetMany<TEntity>(IEnumerable<string> ids, ProjectionDefinition<TEntity> Projection) where TEntity : class, new()
        {

            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.Eq(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ids);
            return await GetMany<TEntity>(filter, Projection);

        }

        /// <summary>
        /// A generic get many method with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<GetManyResult<TEntity>> GetMany<TEntity>(FilterDefinition<TEntity> filter) where TEntity : class, new()
        {
            var res = new GetManyResult<TEntity>();
            try
            {
               

                var collection = GetCollection<TEntity>();
                var entities = new List<TEntity>();
                if (PageSize != 0)
                {
                    if (SortDefinition != null)
                        entities = await collection.Find(filter).Sort(this.ConvertSort<TEntity>()).Skip(CurrentPage * PageSize).Limit(PageSize).ToListAsync();
                    else
                    entities = await collection.Find(filter).Skip(CurrentPage * PageSize).Limit(PageSize).ToListAsync();
                }
                else
                {
                    if (SortDefinition != null)
                        entities = await collection.Find(filter).Sort(this.ConvertSort<TEntity>()).ToListAsync();
                    else
                        entities = await collection.Find(filter).ToListAsync();
                }
               
                if (entities != null)
                {
                    res.Entities = entities;
                }
                res.Success = true;
               
            }
            catch(Exception EX)
            {
                res.Success = false;
            }
            return res;

        }
       

        public async Task<IAggregateFluent<TEntity>> GetManyAggregate<TEntity>() where TEntity : class, new()
        {
            var res = new GetManyResult<TEntity>();
            IAggregateFluent<TEntity> entities = null;
            try
            {


                var collection = GetCollection<TEntity>();

                entities = collection.Aggregate();
                res.Success = true;

            }
            catch (Exception EX)
            {
                res.Success = false;
            }
            return await Task.FromResult(entities);

        }

       

        public async Task<GetManyResult<TEntity>> GetMany<TEntity>(FilterDefinition<TEntity> filter, ProjectionDefinition<TEntity> Projection) where TEntity : class, new()
        {
            var res = new GetManyResult<TEntity>();

            var collection = GetCollection<TEntity>();
            
            var entities = new List<TEntity>();
            if (PageSize != 0)
            {
                entities = await collection.Find(filter).Project<TEntity>(Projection).Skip(CurrentPage * PageSize).Limit(PageSize).ToListAsync();
            }
            else
            {
                entities = await collection.Find(filter).Project<TEntity>(Projection).ToListAsync();
            }
            if (entities != null)
            {
                res.Entities = entities;
            }
            res.Success = true;
            return res;

        }

        /// <summary>
        /// FindCursor
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <returns>A cursor for the query</returns>
        public IFindFluent<TEntity, TEntity> FindCursor<TEntity>(FilterDefinition<TEntity> filter) where TEntity : class, new()
        {
            var res = new GetManyResult<TEntity>();
            var collection = GetCollection<TEntity>();
            var cursor = collection.Find(filter);
            return cursor;
        }

        public SortDefinition<TEntity> ConvertSort<TEntity>()
        {
            var Data = Builders<TEntity>.Sort;
            SortDefinition<TEntity> Sort= null;
            
            if (SortDefinition !=null)
            {
                IEnumerable<SortDefinition<TEntity>> Data2 = new SortDefinition<TEntity>[] {
                 
                };
                List<SortDefinition<TEntity>> Data3 = Data2.ToList();
                
                foreach (SortField Item in SortDefinition)
                {
                    if (Item.Sort == SortField.Direction.Ascending)
                        Data3.Add(Data.Ascending(Item.FieldName));
                    else
                        Data3.Add(Data.Descending(Item.FieldName));
                }
                Sort=Data.Combine(Data3);
            }
            
            return Sort;
        }

     
        /// <summary>
        /// A generic get all method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<GetManyResult<TEntity>> GetAll<TEntity>() where TEntity : class, new()
        {
            var res = new GetManyResult<TEntity>();
            //var _client = new MongoClient(ConfigurationManager.AppSettings["MongoDBContext"].ToString());
            //var _database = _client.GetDatabase("VirtualLearningCenter");
            //var collection =  _database.GetCollection<TEntity>("msEbooks");
            var collection = GetCollection<TEntity>();
           
            try
            {
                var entities = new List<TEntity>();
                if (PageSize !=0)
                { 
                    if (SortDefinition!= null)
                    entities = await collection.Find(new BsonDocument()).Sort(this.ConvertSort<TEntity>()).Skip(CurrentPage * PageSize).Limit(PageSize).ToListAsync();
                    else
                        entities = await collection.Find(new BsonDocument()).Skip(CurrentPage * PageSize).Limit(PageSize).ToListAsync();


                }
                else
                {


                    if (SortDefinition != null)
                    entities = await collection.Find(new BsonDocument()).Sort(this.ConvertSort<TEntity>()).ToListAsync();
                    else
                        entities = await collection.Find(new BsonDocument()).ToListAsync();
                }
               
                if (entities != null)
                {
                    res.Entities = entities;
                }
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message =ex.ToString();
                return res;
            }
            //var collection = GetCollection<TEntity>();
           
            return res;
        }
        public async Task<GetManyResult<TEntity>> GetAll<TEntity>(ProjectionDefinition<TEntity> Projection) where TEntity : class, new()
        {
            var res = new GetManyResult<TEntity>();
            //var _client = new MongoClient(ConfigurationManager.AppSettings["MongoDBContext"].ToString());
            //var _database = _client.GetDatabase("VirtualLearningCenter");
            //var collection =  _database.GetCollection<TEntity>("msEbooks");
            var collection = GetCollection<TEntity>();

            try
            {
               // var entities = await collection.Find(new BsonDocument()).Project<TEntity>(Projection).ToListAsync();
                var entities = new List<TEntity>();
                if (PageSize != 0)
                {
                    entities = await collection.Find(new BsonDocument()).Project<TEntity>(Projection).Skip(CurrentPage * PageSize).Limit(PageSize).ToListAsync();
                }
                else
                {
                    entities = await collection.Find(new BsonDocument()).Project<TEntity>(Projection).ToListAsync();
                }
                if (entities != null)
                {
                    res.Entities = entities;
                }
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.ToString();
                return res;
            }
            //var collection = GetCollection<TEntity>();

            return res;
        }

        /// <summary>
        /// A generic Exists method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Exists<TEntity>(string id) where TEntity : class, new()
        {
            var collection = GetCollection<TEntity>();
            var query = new BsonDocument(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ObjectId.Parse(id));
            var cursor = collection.Find(query);
            var count = await cursor.CountAsync();
            return (count > 0);
        }

        /// <summary>
        /// A generic count method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name=ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString()></param>
        /// <returns></returns>
        public async Task<long> Count<TEntity>(string id) where TEntity : class, new()
        {
            var filter = new FilterDefinitionBuilder<TEntity>().Eq(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ObjectId.Parse(id));
            return await Count<TEntity>(filter);
        }

        /// <summary>
        /// A generic count method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name=ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString()></param>
        /// <returns></returns>
        public async Task<long> Count<TEntity>(FilterDefinition<TEntity> filter) where TEntity : class, new()
        {
            var collection = GetCollection<TEntity>();
            var cursor = collection.Find(filter);
            var count = await cursor.CountAsync();
            //var count = await cursor.ToListAsync();
            return count;
        }
        #endregion Get

        #region Create
        /// <summary>
        /// A generic Add One method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Result> AddOne<TEntity>(TEntity item) where TEntity : class, new()
        {
            var res = new Result();
           
                var collection = GetCollection<TEntity>();
                await collection.InsertOneAsync(item);
                res.Success = true;
                res.Message = "OK";
                return res;
           
        }
        #endregion Create

        #region Delete
        /// <summary>
        /// A generic delete one method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name=ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString()></param>
        /// <returns></returns>
        public async Task<Result> DeleteOne<TEntity>(string id) where TEntity : class, new()
        {
            var filter = new FilterDefinitionBuilder<TEntity>().Eq(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ObjectId.Parse(id));
            return await DeleteOne<TEntity>(filter);
        }

        /// <summary>
        /// A generic delete one method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name=ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString()></param>
        /// <returns></returns>
        public async Task<Result> DeleteOne<TEntity>(FilterDefinition<TEntity> filter) where TEntity : class, new()
        {
            var result = new Result();
            
                var collection = GetCollection<TEntity>();
                var deleteRes = await collection.DeleteOneAsync(filter);
                result.Success = true;
                result.Message = "OK";
                return result;
           
        }

        /// <summary>
        /// A generic delete many method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Result> DeleteMany<TEntity>(IEnumerable<string> ids) where TEntity : class, new()
        {
            var filter = new FilterDefinitionBuilder<TEntity>().In(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ids);
            return await DeleteMany<TEntity>(filter);
        }

        /// <summary>
        /// A generic delete many method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Result> DeleteMany<TEntity>(FilterDefinition<TEntity> filter) where TEntity : class, new()
        {
            var result = new Result();
           
                var collection = GetCollection<TEntity>();
                var deleteRes = await collection.DeleteManyAsync(filter);
                if (deleteRes.DeletedCount < 1)
                {
                  
                    result.Message = "DeleteMany Some " + typeof(TEntity).Name + "s could not be deleted.";
                    return result;
                }
                result.Success = true;
                result.Message = "OK";
                return result;
           
        }
        #endregion Delete

        #region Update
        /// <summary>
        /// UpdateOne by id
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name=ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString()></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateOne<TEntity>(string id, UpdateDefinition<TEntity> update) where TEntity : class, new()
        {
            var filter = new FilterDefinitionBuilder<TEntity>().Eq(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ObjectId.Parse(id));
            return await UpdateOne<TEntity>(filter, update);
        }

        /// <summary>
        /// UpdateOne with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateOne<TEntity>(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update) where TEntity : class, new()
        {
            var result = new Result();
         try
            {
                var collection = GetCollection<TEntity>();
                var updateRes = await collection.UpdateOneAsync(filter, update);
                if (updateRes.ModifiedCount < 1)
                {
                    var ex = new Exception();
                    result.Message = "UpdateOne ERROR: updateRes.ModifiedCount < 1 for entity: " + typeof(TEntity).Name;
                    return result;
                }
                result.Success = true;
                result.Message = "OK";
                return result;
            }
            catch(Exception Ex)
            {
                result.Message = Ex.Message;
                result.Success = false;
                return result;
            }
               
              
            
        }

        /// <summary>
        /// UpdateMany with Ids
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name=ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString()></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateMany<TEntity>(IEnumerable<string> ids, UpdateDefinition<TEntity> update) where TEntity : class, new()
        {
            var filter = new FilterDefinitionBuilder<TEntity>().In(ConfigurationManager.AppSettings["BookDBMongoDBIDTitle"].ToString(), ids);
            return await UpdateOne<TEntity>(filter, update);
        }

        /// <summary>
        /// UpdateMany with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<Result> UpdateMany<TEntity>(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update) where TEntity : class, new()
        {
            var result = new Result();
            
                var collection = GetCollection<TEntity>();
                var updateRes = await collection.UpdateManyAsync(filter, update);
                if (updateRes.ModifiedCount < 1)
                {
                    var ex = new Exception();
                    result.Message = "UpdateMany ERROR: updateRes.ModifiedCount < 1 for entities: " + typeof(TEntity).Name + "s";
                    return result;
                }
                result.Success = true;
                result.Message = "OK";
                return result;
           
        }
        #endregion Update

        #region Find And Update

        /// <summary>
        /// GetAndUpdateOne with filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<GetOneResult<TEntity>> GetAndUpdateOne<TEntity>(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, FindOneAndUpdateOptions<TEntity, TEntity> options) where TEntity : class, new()
        {
            var result = new GetOneResult<TEntity>();
           
                var collection = GetCollection<TEntity>();
                result.Entity = await collection.FindOneAndUpdateAsync(filter, update, options);
                result.Success = true;
                result.Message = "OK";
                return result;
           
        }

        #endregion Find And Update


        /// <summary>
        /// The private GetCollection method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        /// private MongoDbContext _mongoDbContext = null;
        

        private IMongoCollection<TEntity> GetCollection<TEntity>(string Collection = null)
        {
            Collection = this.CollectionName;
            return DataContext.GetCollection<TEntity>(Collection);
        }
        private IMongoCollection<BsonDocument> GetCollection(string Collection = null)
        {
            Collection = this.CollectionName;
            return DataContext.GetCollection<BsonDocument>(Collection);
        }
    }
}
