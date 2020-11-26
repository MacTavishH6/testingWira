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

namespace Binus.SampleWebAPI.WebAPI.App_Start.Routing
{

    public class APPIDFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        //private readonly HttpConfiguration _configuration;
        private readonly IControllerService _controllerService;
        [Dependency]
        IControllerService ControllerService { get; set; }


        public APPIDFilter()
        {

        }
        public override void OnActionExecuting(HttpActionContext ActionContext)
        {
            if (ActionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<NoAPPFilter>(true).Any())
            {
                return;
            }
            IEnumerable<string> Headers = ActionContext.Request.Headers.GetValues("AppID");
            if (Headers != null)
            {
                string Time = Crypto.Decrypt(Headers.FirstOrDefault());
                Time = ApplicationID.DecryptAPPID(Time);
                Int64 APPID = Convert.ToInt64(Time.Substring(12, Time.Length - 12));
                Time = Time.Substring(0, 12);
                if ((Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmm")) - Convert.ToDouble(Time)) > Convert.ToDouble(ConfigurationManager.AppSettings["WebAPIExecTime"]))
                {

                    ActionContext.Response = ActionContext.Request.CreateResponse(
                            System.Net.HttpStatusCode.BadRequest,
                            new { Error = "[error 4]" },
                            ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                        );

                }
                else
                {
                    if (ActionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<SkipAPPFilterAuth>(true).Any())
                    {
                        return;
                    }
                    ControllerFacade _ControllerService = new ControllerFacade();
                    string ActionName = ActionContext.ControllerContext.RouteData.Values["action"].ToString();
                    List<WebAPIController> Auth = _ControllerService.GetControllerAuth(new WebAPIController() { AppID = Convert.ToInt64(APPID), ControllerFullName = ActionContext.ControllerContext.Controller.ToString() }).ToList();

                    if (Auth.Count() == 0)
                    {
                        throw new HttpResponseException(ActionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "[error 3]"));
                        //ActionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                        //{
                        //    Content = new System.Net.Http.StringContent("You're unauthorized to call this API")
                        //};
                        //ActionContext.Response = ActionContext.Request.CreateResponse(
                        //    System.Net.HttpStatusCode.BadRequest,
                        //    new { Error = "You're unauthorized to call this API" },
                        //    ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                        //);
                        //return;

                    }


                }


            }
            else
            {
                ActionContext.Response = ActionContext.Request.CreateResponse(
                            System.Net.HttpStatusCode.BadRequest,
                            new { Error = "[error 3]" },
                            ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                        );

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