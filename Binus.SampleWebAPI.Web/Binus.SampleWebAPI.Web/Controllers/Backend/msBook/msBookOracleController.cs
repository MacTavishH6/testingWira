using System.Web.Mvc;
using Binus.WebAPI.REST;
using Binus.SampleWebAPI.Web.Class;
using Binus.SampleWebAPI.Web.ViewModels.msBooks;
using Binus.SampleWebAPI.Model.Backend.msBook;
using System.Collections.Generic;
using Binus.SampleWebAPI.Model.Base;
using Binus.SampleWebAPI.Model.Backend.User;

namespace Binus.SampleWebAPI.Web.Controllers.Backend.msBook
{
    public class msBookOracleController : Controller
    {
        [App_Start.BasicAuthentication]
        public ActionResult Index()
        {
            RESTResult Result = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Backend/msBookOracle/Get", REST.Method.POST).Result);
            if(Result.Success)
            {
                msBookViewModel ViewModel = new msBookViewModel();
                ViewModel.Books = Result.Deserialize<List<msBookMSSQLOracle>>();
                return View(ViewModel);
            }
            else
            {
                return View();
            }
           

        }

        public ActionResult Manipulation(msBookViewModel Data)
        {
            msUser CurrentUser = (msUser)Session["ActiveUser"];
            Data.Book.UserIn = CurrentUser.Nama;
            Data.Book.UserUp = CurrentUser.Nama;
            RESTResult ResultItem = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Backend/msBookOracle/Manipulation", REST.Method.POST, Data.Book).Result);
            if (ResultItem.Success)
            {
                return Json(new { Result = "Success", URL = "/msBookOracle" });
            }
            else
            {
                return Json(new { Result = "Fail", Message = "Error while manipulate data" });
            }


        }

        public ActionResult Edit(msBookMSSQLOracle Data)
        {
            RESTResult Result = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Backend/msBookOracle/Get", REST.Method.POST, Data).Result);
            if (Result.Success)
            {
                return Json(Result.Deserialize<List<msBookMSSQLOracle>>());
            }
            else
            {
                return View();
            }


        }

        public ActionResult Delete()
        {
            RESTResult Result = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Backend/msBookOracle/Get", REST.Method.POST).Result);
            if (Result.Success)
            {
                msBookViewModel ViewModel = new msBookViewModel();
                ViewModel.Books = Result.Deserialize<List<msBookMSSQLOracle>>();
                return View(ViewModel);
            }
            else
            {
                return View();
            }


        }
    }
}
