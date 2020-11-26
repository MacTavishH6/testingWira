using Binus.WebAPI.Model.Common;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Binus.WebAPI.Cryptography;
using System.Collections.Generic;
using System.Linq;
using Binus.SampleWebAPI.Services.Common;

namespace Binus.SampleWebAPI.WebAPI.Controllers.Base.V1.User
{
    [ApiVersion("1.0")]
    //[Route("api/Base/v{version:apiVersion}/BinusianPhoto/binusianPhoto")]
    //[Authorize]
    public class UserController : ApiController
    {
      
        private IUserHRISDBService _userHRISDBService;

        public UserController(IUserHRISDBService HRISDBService)
        {
            _userHRISDBService = HRISDBService; 
        }

        [HttpPost]
        public async Task<IHttpActionResult> Get(UserHRISDB Data)
        {
          List<UserHRISDB> Return = (await _userHRISDBService.GetEmployeeHRISByKeyAsync(Crypto.Decrypt(Data.Username))).ToList();
            
          
            return Json(Return);
        }
        [HttpPost]
        public async Task<IHttpActionResult> GetEmployeeDataByEmailOrBinusianIDAsync(UserHRISDB Data)
        {
            string BinusianID = (Data.Binusian_ID == null ? "": Crypto.Decrypt(Data.Binusian_ID));
            string Email = (Data.Email == null ? "" : Crypto.Decrypt(Data.Email));
            EmployeeDB Return = (await _userHRISDBService.GetEmployeeDataByEmailOrBinusianIDAsync(BinusianID,Email));


            return Json(Return);
        }

       

    }
}
