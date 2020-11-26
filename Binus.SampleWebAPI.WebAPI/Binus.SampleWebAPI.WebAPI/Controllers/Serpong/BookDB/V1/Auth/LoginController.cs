using Binus.WebAPI.Cryptography;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Binus.WebAPI.Model.Common;
using Binus.SampleWebAPI.Services.Common;
using Binus.SampleWebAPI.Services.Base;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MSSQL.Backend;
using Binus.SampleWebAPI.Services.Serpong.BookDBAPP.MSSQL.Backend;
using System.DirectoryServices;


namespace Binus.ResourcesCenter.WebAPI.Controllers.Serpong.BookDB.V1.Auth
{
    [ApiVersion("1.0")]
    [Authorize]
    public class LoginController : ApiController
    {
      
        private IUserHRISDBService _userHRISDBService;

        private ImsUserService _msUserService;
        public LoginController( IUserHRISDBService HRISDBService, ImsUserService msUserService)
        {

            _userHRISDBService = HRISDBService;
            _msUserService = msUserService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> DoLogin(UserHRISDB Data)
        {
            EmployeeDB Return = new EmployeeDB();
            try
            {
                var LDAPService = new LDAPService();
                SearchResult UserAD = (SearchResult)LDAPService.LDAPUserSearch(Crypto.Decrypt(Data.Username.Replace(" ", "+")), SearchType.One, SearchBy.Username, Crypto.Decrypt(Data.Username.Replace(" ", "+")), Crypto.Decrypt(Data.Password.Replace(" ", "+")));

                if (UserAD != null)
                {
                    Return = new EmployeeDB();
                    ResultPropertyCollection UserData = UserAD.Properties;
                    Return.Nama = UserData["name"][0].ToString();
                    Return.Email = UserData["mail"][0].ToString();
                    Return.Email1 = UserData["mail"][0].ToString();
                    Return.NM_Jabatan = UserData["title"][0].ToString();
                    Return.JobName = UserData["title"][0].ToString();
                    Return.NM_Atasan_Langsung = (UserData["manager"][0].ToString() != "" ?
                        (UserData["manager"][0].ToString().ToLower().Contains("cn") ?
                            UserData["manager"][0].ToString().Split(',')[0].Replace("CN", "") : "")
                        : "");
                    Return.Nama_DEP = UserData["department"][0].ToString();
                    Return.Nama_Div = UserData["company"][0].ToString();
                    Return.Nama_BU = UserData["company"][0].ToString();
                    Return.Binusian_ID = UserData["extensionattribute10"][0].ToString();
                    Return.Lokasi_Kerja = UserData["physicaldeliveryofficename"][0].ToString();
                }

                //foreach (Hashtable Item in UserData)
                //    {

                //    }

                //Return.Nama = UserAD.Properties.PropertyNames;
                //Return.Email = UserAD.EmailAddress;
                //Return.NM_Jabatan = "";
            }
            catch
            {

            }
            return await Task.FromResult(Json(Return));
        }

        [HttpPost]
        public async Task<IHttpActionResult> DoLoginDB(msUser Data)
        {

            msUser Return = await _msUserService.GetOne(Data);
            
            return Json(Return);
        }


    }
}
