using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.Oracle.Backend;
using Binus.SampleWebAPI.Services.Serpong.BookDBAPP.Oracle.Backend;
using Binus.WebAPI.Cryptography;
using Binus.WebAPI.Model.MSSQL;
using Microsoft.Web.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Binus.ResourcesCenter.WebAPI.Controllers.Serpong.BookDB.V1.Backend
{
    [ApiVersion("1.0")]
   [Authorize]
    public class msBookOracleController : ApiController
    {
      
        private ImsBookOracleService _msBookService;


        public msBookOracleController(ImsBookOracleService msBookService)
        {

            this._msBookService = msBookService;
           
        }

       
        [HttpPost]
        public async Task<IHttpActionResult> Get(msBook Data)
        {
            int ID = 0;
            if(Data !=null)
            {
                if (Data.IDEncrypt != "")
                {
                    ID = Convert.ToInt32(Crypto.Decrypt(Data.IDEncrypt.Replace(" ", "+")));
                }
            }
           
            List<msBook> Result = (await _msBookService.GetBook(ID)).ToList();

          
            return Json(Result);
        }



        [HttpPost]
        public async Task<IHttpActionResult> Manipulation(msBook Data)
        {
            ExecuteResult Result = new ExecuteResult();
            try
            {

                if (Data.Stsrc == "I")
                {
                    Result = (await _msBookService.InsertBook(Data));
                }
                else if (Data.Stsrc == "U")
                {
                    Data.ID = Convert.ToInt32(Crypto.Decrypt(Data.IDEncrypt.Replace(" ", "+")));
                    Result = (await _msBookService.UpdateBook(Data));
                }
                else if (Data.Stsrc == "D")
                {
                    Data.ID = Convert.ToInt32(Crypto.Decrypt(Data.IDEncrypt.Replace(" ", "+")));
                    Result = (await _msBookService.DeleteBook(Data));
                }


            }
            catch
            {
                Result.Status = false;
                Result.Message = "Invalid parameter";
            }
            return Json(Result);
        }
        }
}
