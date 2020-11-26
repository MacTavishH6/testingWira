using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Binus.WebAPI.Cryptography;
using Binus.SampleWebAPI.Model.Base;
using Binus.SampleWebAPI.Model.Common;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MongoDB.Backend;
using Binus.SampleWebAPI.Data.Repositories.Serpong.MongoDB.Backend;

namespace Binus.SampleWebAPI.Services.Serpong.BookDBAPP.MongoDB.Backend
{
    public interface ImsBookMongoDBService
    {

        Task<IEnumerable<msBook>> GetBook(string ID = "");
      
        Task<Result> InsertBook(msBook Data);
        Task<Result> UpdateBook(msBook Data);
        Task<Result> DeleteBook(msBook Data);

    }
    public class msBookService : ImsBookMongoDBService
    {

        private readonly ImsBookMongoDBRepository _msBookMongoDBRepository;
       

        public msBookService(ImsBookMongoDBRepository msBookMongoDBRepository)
        {
          
            this._msBookMongoDBRepository = msBookMongoDBRepository;
          
        }

        public async Task<IEnumerable<msBook>> GetBook(string ID="")
        {
          
            List<SortField> Sort = new List<SortField>() { new SortField() { FieldName = "BookName", Sort = SortField.Direction.Ascending }};
            await _msBookMongoDBRepository.SortData(Sort);

            try
            {
                List<msBook> ReturnData;
              if (ID != "")
                {
                    var Builder = Builders<msBook>.Filter;
                    var FilterData =Builder.And(Builder.Eq("_id", ObjectId.Parse(ID)), Builder.Ne("Stsrc", "D"));
                    
                    ReturnData = (await _msBookMongoDBRepository.GetMany<msBook>(FilterData)).Entities.ToList();
                }
                else
                {
                    var Builder = Builders<msBook>.Filter;
                    var FilterData = Builder.Ne("Stsrc", "D");

                    ReturnData = (await _msBookMongoDBRepository.GetMany<msBook>(FilterData)).Entities.ToList();


                }

                IEnumerable<msBook> Data2 = BsonSerializer.Deserialize<List<msBook>>(ReturnData.ToJson());
                return Data2;
            }
            catch
            {
                return null;
            }


        }


        public async Task<Result> InsertBook(msBook Data)
        {
            Result Result = new Result();
            try
            {
                var Builder = Builders<msBook>.Filter;
                var filter = new BsonDocument { };
              
                Result = (await _msBookMongoDBRepository.AddOne<msBook>(Data));

            }
            catch (Exception EX)
            {
                Result.Success = false;
                Result.Message = EX.Message.ToString();

            }
            return Result;
        }

        public async Task<Result> UpdateBook(msBook Data)
        {
            Result Result = new Result();
            try
            {

                var Update = Builders<msBook>.Update
                    .Set("Stsrc", "U")
                    .Set("UserUp", Data.UserUp)
                    .Set("DateUp", Data.DateUp)
                    .Set("ISBN", Data.ISBN)
                    .Set("BookName", Data.BookName)
                    .Set("Author", Data.Author)
                    .Set("Publisher", Data.Publisher);
                Result = (await _msBookMongoDBRepository.UpdateOne<msBook>(Data._id.ToString(), Update));


            }
            catch (Exception EX)
            {
                Result.Success = false;
                Result.Message = EX.Message.ToString();

            }
            return Result;
        }

        public async Task<Result> DeleteBook(msBook Data)
        {
            Result Result = new Result();
            try
            {

                var Update = Builders<msBook>.Update
                  .Set("Stsrc", "D")
                  .Set("UserUp", Data.UserUp)
                  .Set("DateUp", Data.DateUp);
                Result = (await _msBookMongoDBRepository.UpdateOne<msBook>(Data._id.ToString(), Update));

            }
            catch (Exception EX)
            {
                Result.Success = false;
                Result.Message = EX.Message.ToString();

            }
            return Result;
        }

       

    }
}
