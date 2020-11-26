using System;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using Binus.WebAPI.REST;
using Binus.WebAPI.Cryptography;
using Binus.WebAPI.Model.Common;
using System.Configuration;
using Binus.SampleWebAPI.Web.Class;
using Binus.SampleWebAPI.Web.ViewModels.Users;
using Binus.SampleWebAPI.Model.Backend.User;

namespace Binus.SampleWebAPI.Web.Controllers.Login
{
    public class LoginController : Controller
    {
      
        public LoginController()
        {
           

        }
        // GET: Login
        public ActionResult Index()
        {
          
            //if (Session["ActiveUser"] == null)
            //{
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Exam");
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Auth(UserViewModel Model)
        {
            string URL;
            //URL = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~") + "MyExam/Index?id=" + HttpUtility.UrlEncode("Oo8qXpjbs2E0Mpkh/8Pq8TgjUNuQF5EWXdfOLKfXwEA=") + "&Page=0";
            URL = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~") + "msBookMSSQL";
            StringBuilder RetValue = new StringBuilder();
            JsonResult RetData = new JsonResult();
            if (Session["ActiveUser"] == null)
            {
                try
                {
                  
                    RESTResult Result = new RESTResult();
                    if (Request.Form["LDAP"]!=null)
                    {
                        UserHRISDB Data = new UserHRISDB();
                        Data.Username = Crypto.Encrypt(Model.UserName);
                        Data.Password = Crypto.Encrypt(Model.Password);
                        Result = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Auth/Login/DoLogin", REST.Method.POST, Data).Result);
                    }
                    else if (Request.Form["DB"] != null)
                    {
                        AuthUser Data = new AuthUser();
                        Data.Username = Crypto.Encrypt(Model.UserName);
                        Data.Password = Crypto.Encrypt(Model.Password);
                        Result = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Auth/Login/DoLoginDB", REST.Method.POST,ConfigurationManager.AppSettings["OAuthBookDB"].ToString(), Data).Result);
                    }
                   
                  

                    if (Result.Success)
                    {
                        msUser User = new msUser();
                        if (Request.Form["LDAP"] != null)
                        {
                            UserHRISDB UserHRISDB = Result.Deserialize<UserHRISDB>();
                            User.Nama = UserHRISDB.Nama;
                            User.Email = UserHRISDB.Email1;
                        }
                        else
                        {
                            User = Result.Deserialize<msUser>();
                            User.Nama = User.Username;
                        }
                        if (User != null)
                        {
                            #region Internal User
                          
                            Session["ActiveUser"] = User;
                          

                            RetData = Json(new { Result = "Success", Message = "Success Login, Please wait, we directing you to main page", URL = URL });
                            #endregion

                        }
                        else
                        {
                            RetData = Json(new { Result = "Fail", Message = "Username or password invalid" });
                        }
                    }
                    else
                    {
                        if(Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            RetData = Json(new { Result = "Fail", Message = "Username or password invalid" });
                        }
                        else if (Result.JSON.Contains("Authorization"))
                        {
                            RetData = Json(new { Result = "Fail", Message = "Username or password invalid" });
                        }
                        else
                        {
                            RetData = Json(new { Result = "Fail", Message = "Server error, please contact your administrator" });
                        }
                       
                    }
                   

                }
                catch (Exception Ex)
                {

                    RetData = Json(new { Result = "Fail", Message = "Error Login, with error: " + Ex.Message.ToString() });
                }
            }
            else
            {
                RetData = Json(new { Result = "Success", Message = "Success Login, Please wait, we directing you to main page", URL = URL });
            }
            

           
            return RetData;
        }
        public async Task<ActionResult> Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

    }
}