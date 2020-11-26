using Binus.SampleWebAPI.Model.Base;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MongoDB.Backend;
using Binus.SampleWebAPI.Services.Serpong.BookDBAPP.MongoDB.Backend;
using Binus.WebAPI.Cryptography;
using Binus.WebAPI.Model.MSSQL;
using Microsoft.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Binus.WebAPI.MongoDB;
using Binus.WebAPI.Utility.DateTime;

namespace Binus.ResourcesCenter.WebAPI.Controllers.Serpong.BookDB.V1.Backend
{
    [ApiVersion("1.0")]
   [Authorize]
    public class msBookMongoDBController : ApiController
    {
      
        private ImsBookMongoDBService _msBookService;


        public msBookMongoDBController(ImsBookMongoDBService msBookService)
        {

            this._msBookService = msBookService;
           
        }

       
        [HttpPost]
        public async Task<IHttpActionResult> Get(msBook Data)
        {
            string ID = "";
            if(Data !=null)
            {
                if (Data.IDEncrypt != "")
                {
                    ID = Crypto.Decrypt(Data.IDEncrypt.Replace(" ", "+"));
                }
            }
           
            List<msBook> Result = (await _msBookService.GetBook(ID)).ToList();

          
            return Json(BSONToJSON.Pretty<List<msBook>>(Result));
        }



        [HttpPost]
        public async Task<IHttpActionResult> Manipulation(msBook Data)
        {
            Result MongoResult = new Result();
            try
            {

                if (Data.Stsrc == "I")
                {
                    Data.DateIn = JakartaTime.Now();
                    Data.DateUp = JakartaTime.Now();
                    MongoResult = (await _msBookService.InsertBook(Data));
                }
                else if (Data.Stsrc == "U")
                {
                    Data.DateUp = JakartaTime.Now();
                    Data._id = ObjectId.Parse(Crypto.Decrypt(Data.IDEncrypt.Replace(" ","+")));
                    MongoResult = (await _msBookService.UpdateBook(Data));
                }
                else if (Data.Stsrc == "D")
                {
                    Data.DateUp = JakartaTime.Now();
                    Data._id = ObjectId.Parse(Crypto.Decrypt(Data.IDEncrypt.Replace(" ", "+")));
                    MongoResult = (await _msBookService.DeleteBook(Data));
                }


            }
            catch
            {
                MongoResult.Success = false;
                MongoResult.Message = "Invalid parameter";
            }
            return Json(MongoResult);
        }
        }
}
