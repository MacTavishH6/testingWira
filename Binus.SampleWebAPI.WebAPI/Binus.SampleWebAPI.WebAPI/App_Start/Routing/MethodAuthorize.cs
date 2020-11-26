using Binus.SampleWebAPI.Services.BinusWebAPI.Common.MSSQL.SyncControllerService;
using Binus.WebAPI.Cryptography;
using Binus.WebAPI.Model.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Binus.SampleWebAPI.WebAPI
{

    public class MethodAuthorize : System.Web.Http.Filters.ActionFilterAttribute
    {
        //private readonly HttpConfiguration _configuration;
        private readonly IControllerService _controllerService;
        [Dependency]
        IControllerService ControllerService { get; set; }


        public MethodAuthorize()
        {
            
        }
        public override void OnActionExecuting(HttpActionContext ActionContext)
        {
            IEnumerable<string> Headers = ActionContext.Request.Headers.GetValues("AppID");
            if (Headers != null)
            {
                string Time = Crypto.Decrypt(Headers.FirstOrDefault());
                Time = ApplicationID.DecryptAPPID(Time);
                int APPID = Convert.ToInt32(Time.Substring(12, Time.Length - 12));
                Time = Time.Substring(0, 12);
                if ((Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmm")) - Convert.ToDouble(Time)) > Convert.ToDouble(ConfigurationManager.AppSettings["WebAPIExecTime"]))
                {

                    ActionContext.Response = ActionContext.Request.CreateResponse(
                            System.Net.HttpStatusCode.BadRequest,
                            new { Error = "Invalid APPID" },
                            ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                        );

                }
                else
                {
                    ControllerFacade _ControllerService = new ControllerFacade();
                    string Method = ActionContext.ControllerContext.RouteData.Values["action"].ToString();
                    string URL = ActionContext.Request.RequestUri.AbsoluteUri.ToString();
                    List<WebAPIController> Auth = _ControllerService.GetMethodAuth(new WebAPIController() { AppID = Convert.ToInt32(APPID), ControllerFullName = ActionContext.ControllerContext.Controller.ToString(), ControllerMethod = Method }).ToList();

                    if (Auth.Count() == 0)
                    {
                        throw new HttpResponseException(ActionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "You not authorized to access method : " + Method + ", in requested url " + URL));


                    }

                }

            }    
               

        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            //if (objectContent != null)
            //{
            //    var type = objectContent.ObjectType; //type of the returned object
            //    var value = objectContent.Value; //holding the returned value
            //}

            //Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());
        }
    }
}