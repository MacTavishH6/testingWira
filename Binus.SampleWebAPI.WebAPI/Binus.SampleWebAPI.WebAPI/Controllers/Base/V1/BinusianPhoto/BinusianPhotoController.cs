using Binus.WebAPI.Model.Common;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Binus.WebAPI.Cryptography;
using Binus.SampleWebAPI.Services.Common;

namespace Binus.SampleWebAPI.WebAPI.Controllers.Base.V1.BinusianPhoto
{
    [ApiVersion("1.0")]
    //[Route("api/Base/v{version:apiVersion}/BinusianPhoto/binusianPhoto")]
    //[Authorize]
    public class BinusianPhotoController : ApiController
    {
      
        private IUserHRISDBService _userHRISDBService;

        public BinusianPhotoController(IUserHRISDBService HRISDBService)
        {

            _userHRISDBService = HRISDBService;
           
        }

        [HttpPost]
        public async Task<IHttpActionResult> Get(UserHRISDB Data)
        {
            Binus.WebAPI.Model.Common.BinusianPhoto Return = _userHRISDBService.GetBinusianPicture(Crypto.Decrypt(Data.Binusian_ID));

            return await Task.FromResult(Json(Return));
        }

    }
}
