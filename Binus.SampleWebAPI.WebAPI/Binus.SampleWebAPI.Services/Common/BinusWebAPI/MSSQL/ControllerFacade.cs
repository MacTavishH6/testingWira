using System.Collections.Generic;
using Binus.WebAPI.Model.Common;
using System.Data.SqlClient;
using Binus.SampleWebAPI.Data.Repositories.Common.MSSQL;
using Binus.SampleWebAPI.Data.Infrastructures.Common.MSSQL;

namespace Binus.SampleWebAPI.Services.BinusWebAPI.Common.MSSQL.SyncControllerService
{

    public class ControllerFacade
    {
      
        private readonly IControllerRepository ControllerRepository;



        public ControllerFacade()
        {
           // RolesRepository.ConnectionString("DeveloperCompetencyDBEntities");
            this.ControllerRepository = new ControllerRepository(new BinusWebAPIDBFactory());

        }
       

        public IEnumerable<WebAPIController> GetControllerAuth(WebAPIController Data)
        {
            var Param = new[] { new SqlParameter("@AppID", Data.AppID.ToString()), new SqlParameter("@ControllerFullName", Data.ControllerFullName) };
            return  ControllerRepository.ExecSPToList("bn_Controller_GetControllerAuth @AppID, @ControllerFullName", Param);
        }

        public IEnumerable<WebAPIController> GetMethodAuth(WebAPIController Data)
        {
            var Param = new[] { new SqlParameter("@AppID", Data.AppID.ToString()), new SqlParameter("@ControllerFullName", Data.ControllerFullName), new SqlParameter("@ControllerMethod", Data.ControllerMethod) };
            return ControllerRepository.ExecSPToList("bn_Controller_GetMethodAuth @AppID, @ControllerFullName, @ControllerMethod", Param);
        }



    }
}
