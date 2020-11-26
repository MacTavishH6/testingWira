using System.Collections.Generic;
using System.Threading.Tasks;
using Binus.WebAPI.Model.MSSQL;
using Binus.WebAPI.MSSQL;
using Binus.WebAPI.Model.Common;
using System.Data.SqlClient;
using System;
using Binus.SampleWebAPI.Data.Repositories.Common.MSSQL;

namespace Binus.SampleWebAPI.Services.BinusWebAPI.Common.MSSQL.SyncControllerService
{
    public interface IControllerService
    {

        Task<IEnumerable<WebAPIController>> Get();
        Task<Tuple<WebAPIController, object>> GetControllerByWebAPIID(string WebAPIID);
        Task<IEnumerable<WebAPIController>> GetControllerAuth(WebAPIController Data);
        Task<ExecuteResult> Insert(List<WebAPIController> Data);
       


    }
    public class ControllerService : IControllerService
    {

        private readonly IControllerRepository ControllerRepository;



        public ControllerService(IControllerRepository ControllerRepository)
        {
           // RolesRepository.ConnectionString("DeveloperCompetencyDBEntities");
            this.ControllerRepository = ControllerRepository;

        }

        public async Task<IEnumerable<WebAPIController>> Get()
        {
            return await ControllerRepository.ExecSPToListAsync("bn_Controller_GetControllerByAPPID");
        }

        public async Task<Tuple<WebAPIController,object>> GetControllerByWebAPIID(string WebAPIID)
        {
           
            var Param = new[] { new SqlParameter("@WebAPIID", WebAPIID), new SqlParameter() { ParameterName = "ControllerFullNameOUT", SqlDbType = System.Data.SqlDbType.VarChar, Value = "", Size = 5, Direction = System.Data.ParameterDirection.Output } };
            return await Task.FromResult(ControllerRepository.ExecSPToSingleWithOutput("EXEC bn_Controller_GetControllerByWebAPIID @WebAPIID, @ControllerFullName=@ControllerFullNameOUT OUTPUT", Param));
        }

        public async Task<IEnumerable<WebAPIController>> GetControllerAuth(WebAPIController Data)
        {
            var Param = new[] { new SqlParameter("@AppID", Data.AppID.ToString()), new SqlParameter("@ControllerFullName", Data.ControllerFullName) };
            return await ControllerRepository.ExecSPToListAsync("bn_Controller_GetControllerAuth", Param);
        }

        public async Task<ExecuteResult> Insert(List<WebAPIController> Data)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> SP = new List<StoredProcedure>();
            ModelSQLParamService<WebAPIController> Param = new ModelSQLParamService<WebAPIController>();

            StoredProcedure SPItem = new StoredProcedure() ;
            SPItem.SQLParamString = "@WebAPIID=" + Data[0].WebAPIID.ToString() + "";
            SP.Add(new StoredProcedure { SPName = "bn_Controller_DeleteController " + SPItem.SQLParamString });
            foreach (WebAPIController Item in Data)
            {
                SPItem = Param.ConvertInSingleLineParam(Item, ParameterType.ExcludeNull);
              
                SP.Add(new StoredProcedure { SPName = "bn_Controller_InsertController " + SPItem.SQLParamString });
            }
        
            ReturnValue =  ControllerRepository.ExecMultipleSPWithTransaction(SP);
            return await Task.FromResult(ReturnValue);
        }
    }
}
