using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Binus.WebAPI.Model.Common;
using Binus.WebAPI.Model.MSSQL;
using System.Configuration;
using Binus.SampleWebAPI.Services.BinusWebAPI.Common.MSSQL.SyncControllerService;

namespace Binus.SampleWebAPI.WebAPI.Controllers.Base.V1.SyncController
{
    [ApiVersion("1.0")]
    //[Route("api/Base/Internal/v{version:apiVersion}/SyncController/Sync2")]
   // [Authorize]
    public class SyncController : ApiController
    {
        IControllerService ControllerService;
       
        public SyncController(IControllerService ControllerService)
        {
            this.ControllerService = ControllerService;
            //this.Config = Config;
        }
        [HttpPost]
        public async Task<IHttpActionResult> GetControllerByWebAPPID()
        {
            Tuple<WebAPIController, object> Data = await ControllerService.GetControllerByWebAPIID("1");
            return Json(Data.Item1);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Exec()
        {
            HttpConfiguration Config = new HttpConfiguration();
            IAssembliesResolver AssembliesResolver = Config.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver ControllersResolver = Config.Services.GetHttpControllerTypeResolver();

            ICollection<Type> ControllerTypes = ControllersResolver.GetControllerTypes(AssembliesResolver);

            List<WebAPIController> Data = new List<WebAPIController>();
            WebAPIController Item;
            UInt64 N = 1;
            foreach (Type T in ControllerTypes)
            {
                
                Item = new WebAPIController();
              
                var Segments = T.Namespace.Split(Type.Delimiter);
                Item.WebAPIID = Convert.ToInt32(ConfigurationManager.AppSettings["WebAPIID"].ToString()) ;
                Item.IDController = Convert.ToUInt64(Item.WebAPIID.ToString() + N.ToString());
                Item.ControllerFullName = T.FullName;
                Item.ControllerFullNameSpace = String.Join(".", Segments.Select((c, i) => new { Name = c, Index = i })
         .Where(x => x.Index < 5)
         .Select(x => x.Name));
                Item.ControllerAPPNameSpace =Segments[Segments.Length - 3];
                Item.ControllerVersion = (Segments[Segments.Length - 2].Contains("V") && Segments[Segments.Length - 2].Length <= 5 ? Segments[Segments.Length - 2] : "");
                Item.ControllerModule = (Segments[Segments.Length - 2].Contains("V") && Segments[Segments.Length - 2].Length <=5 ? Segments[Segments.Length - 1] : "");
                Item.ControllerName = T.Name;
               
                Item.SyncByApps = "1";
                Item.UserIn = "ResourcesCenterWebAPI";
                Data.Add(Item);
                N++;
            }

            try
            {
                ExecuteResult Result = await ControllerService.Insert(Data);
            }
            catch(Exception EX)
            {

            }
            return await Task.FromResult(Json(Data));
        }
    }
}
