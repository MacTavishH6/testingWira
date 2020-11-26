using System.Collections.Generic;
using System.Threading.Tasks;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MSSQL.Backend;
using Binus.SampleWebAPI.Data.Repositories.Serpong.MSSQL.Backend;
using Binus.WebAPI.Model.MSSQL;
using Binus.WebAPI.MSSQL;
using System.Data.SqlClient;

namespace Binus.SampleWebAPI.Services.Serpong.BookDBAPP.MSSQL.Backend
{
    public interface ImsBookMSSQLService
    {
        Task<IEnumerable<msBook>> GetBook(int ID = 0);

        Task<ExecuteResult> InsertBook(msBook Data);
        Task<ExecuteResult> UpdateBook(msBook Data);
        Task<ExecuteResult> DeleteBook(msBook Data);


    }
    public class msBookService : ImsBookMSSQLService
    {

        private readonly ImsBookMSSQLRepository _msBookMSSQLRepository;



        public msBookService(ImsBookMSSQLRepository msBookMSSQLRepository)
        {
           // RolesRepository.ConnectionString("DeveloperCompetencyDBEntities");
            this._msBookMSSQLRepository = msBookMSSQLRepository;
        }

        public async Task<IEnumerable<msBook>> GetBook(int ID = 0)
        {
            var Param = new[] { new SqlParameter("@ID", ID) };
            return await _msBookMSSQLRepository.ExecSPToListAsync("bn_Book_GetBook @ID", Param);
        }

        public async Task<ExecuteResult> InsertBook(msBook Data)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> SPList= new List<StoredProcedure>();
            ModelSQLParamService<msBook> Param = new ModelSQLParamService<msBook>();
            string[] ExIncludeProperty = new string[2];
            ExIncludeProperty[0] = "ID";
            ExIncludeProperty[1] = "IDEncrypt";
            StoredProcedure SP;
            SP = Param.ConvertInSingleLineParam(Data, ParameterType.ExcludeNull, ExIncludeProperty);
            SPList.Add(new StoredProcedure { SPName = "bn_Book_InsertBook " + SP.SQLParamString });
      
            ReturnValue = await _msBookMSSQLRepository.ExecMultipleSPWithTransactionAsync(SPList);

            return ReturnValue;  
        }

        public async Task<ExecuteResult> UpdateBook(msBook Data)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> SPList = new List<StoredProcedure>();
            ModelSQLParamService<msBook> Param = new ModelSQLParamService<msBook>();

            string[] ExIncludeProperty = new string[2];
            ExIncludeProperty[0] = "IDEncrypt";
            ExIncludeProperty[1] = "UserIn";
            

            StoredProcedure SP;
            SP = Param.ConvertInSingleLineParam(Data, ParameterType.ExcludeNull, ExIncludeProperty);
            SPList.Add(new StoredProcedure { SPName = "bn_Book_UpdateBook " + SP.SQLParamString });

            ReturnValue = await _msBookMSSQLRepository.ExecMultipleSPWithTransactionAsync(SPList);

            return ReturnValue;
        }

        public async Task<ExecuteResult> DeleteBook(msBook Data)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> SPList = new List<StoredProcedure>();
            ModelSQLParamService<msBook> Param = new ModelSQLParamService<msBook>();

            string[] ExIncludeProperty = new string[3];
            ExIncludeProperty[0] = "IDEncrypt";
            ExIncludeProperty[1] = "UserIn";
            ExIncludeProperty[2] = "Stsrc";

            StoredProcedure SP;
            SP = Param.ConvertInSingleLineParam(Data, ParameterType.ExcludeNull, ExIncludeProperty);
            SPList.Add(new StoredProcedure { SPName = "bn_Book_DeleteBook " + SP.SQLParamString });

            ReturnValue = await _msBookMSSQLRepository.ExecMultipleSPWithTransactionAsync(SPList);

            return ReturnValue;
        }
    }
}
