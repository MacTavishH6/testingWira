using System.Web.Mvc;
using Binus.WebAPI.REST;
using Binus.SampleWebAPI.Web.Class;
using Binus.SampleWebAPI.Web.ViewModels.msBooks;
using System.Collections.Generic;
using Binus.SampleWebAPI.Model.Backend.User;
using Binus.SampleWebAPI.Model.Backend.msBook;

namespace Binus.SampleWebAPI.Web.Controllers.Backend.msBook
{
    [App_Start.BasicAuthentication]
    public class msBookMongoDBController : Controller
    {
        public ActionResult Index()
        {
            RESTResult Result = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Backend/msBookMongoDB/Get", REST.Method.POST).Result);
            if(Result.Success)
            {
                msBookViewModel ViewModel = new msBookViewModel();
                ViewModel.BooksMongoDB = Result.Deserialize<List<msBookMonggoDB>>();
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
            Data.BookMongoDB.UserIn = CurrentUser.Nama;
            Data.BookMongoDB.UserUp = CurrentUser.Nama;
            RESTResult ResultItem = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Backend/msBookMongoDB/Manipulation", REST.Method.POST, Data.BookMongoDB).Result);
            if (ResultItem.Success)
            {
                return Json(new { Result = "Success", URL = "/msBookMongoDB" });
            }
            else
            {
                return Json(new { Result = "Fail", Message = "Error while manipulate data" });
            }


        }

        public ActionResult Edit(msBookMonggoDB Data)
        {
            RESTResult Result = (new REST(Global.WebAPIBaseURL, "/api/Serpong/BookDB/v1/Backend/msBookMongoDB/Get", REST.Method.POST, Data).Result);
            if (Result.Success)
            {
                return Json(Result.Deserialize<List<msBookMonggoDB>>());
            }
            else
            {
                return View();
            }


        }

    }
}
