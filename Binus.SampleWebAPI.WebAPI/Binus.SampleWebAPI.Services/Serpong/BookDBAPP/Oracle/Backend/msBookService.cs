using System.Collections.Generic;
using System.Threading.Tasks;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.Oracle.Backend;
using Binus.SampleWebAPI.Model.Base;
using Binus.SampleWebAPI.Data.Repositories.Serpong.Oracle.Backend;
using System;
using Binus.WebAPI.Model.MSSQL;
using Binus.WebAPI.Model.Oracle;
using Binus.WebAPI.MSSQL;

namespace Binus.SampleWebAPI.Services.Serpong.BookDBAPP.Oracle.Backend
{
    public interface ImsBookOracleService
    {
        Task<IEnumerable<msBook>> GetBook(int ID = 0);

        Task<ExecuteResult> InsertBook(msBook Data);
        Task<ExecuteResult> UpdateBook(msBook Data);
        Task<ExecuteResult> DeleteBook(msBook Data);


    }
    public class msBookService : ImsBookOracleService
    {

        private readonly ImsBookOracleRepository _msBookOracleRepository;



        public msBookService(ImsBookOracleRepository _msBookOracleRepository)
        {
           // RolesRepository.ConnectionString("DeveloperCompetencyDBEntities");
            this._msBookOracleRepository = _msBookOracleRepository;

        }


        public async Task<IEnumerable<msBook>> GetBook(int ID=0)
        {
            if(ID!=0)
            {
                return await _msBookOracleRepository.ExecSQLToListAsync("SELECT ID,ISBN,BOOKNAME,AUTHOR,PUBLISHER,STSRC,USERIN,USERUP,DATEIN,DATEUP FROM MSBOOK WHERE ID="+ID+ " AND STSRC<>'D' ORDER BY BOOKNAME ASC");
            }
            else
            {
                return await _msBookOracleRepository.ExecSQLToListAsync("SELECT ID,ISBN,BOOKNAME,AUTHOR,PUBLISHER,STSRC,USERIN,USERUP,DATEIN,DATEUP FROM MSBOOK WHERE STSRC <>'D' ORDER BY BOOKNAME ASC");
            }
        }
        public async Task<ExecuteResult> InsertBook(msBook Data)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<BulkSQL> SPList = new List<BulkSQL>();
            ModelSQLParamService<msBook> Param = new ModelSQLParamService<msBook>();

            StoredProcedure SP;
            SP = Param.ConvertInSingleLineParam(Data, ParameterType.ExcludeNull);
            string Query = @"INSERT INTO MSBOOK(ISBN,BOOKNAME,AUTHOR,PUBLISHER,STSRC,USERIN,USERUP,DATEIN,DATEUP) VALUES('" + Data.ISBN + @"','" + Data.BookName + @"','" + Data.Author + @"','" + Data.Publisher + @"','" + Data.Stsrc + @"','" + Data.UserIn + @"','" + Data.UserUp + @"',(SELECT SYSDATE FROM DUAL),(SELECT SYSDATE FROM DUAL))";
            SPList.Add(new BulkSQL { SQL = Query });

            ReturnValue = await _msBookOracleRepository.ExecMultipleSQLWithTransactionAsync(SPList);

            return ReturnValue;
          

        }
        public async Task<ExecuteResult> UpdateBook(msBook Data)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<BulkSQL> SPList = new List<BulkSQL>();
            ModelSQLParamService<msBook> Param = new ModelSQLParamService<msBook>();

            StoredProcedure SP;
            SP = Param.ConvertInSingleLineParam(Data, ParameterType.ExcludeNull);
            string Query = @"UPDATE MSBOOK SET 
                                ISBN='" + Data.ISBN + @"',
                                BOOKNAME='" + Data.BookName + @"',
                                AUTHOR='" + Data.Author + @"',
                                PUBLISHER='" + Data.Publisher + @"',
                                STSRC='" + Data.Stsrc + @"',
                                USERUP='" + Data.UserUp + @"',
                                DATEUP=(SELECT SYSDATE FROM DUAL) 
                            WHERE ID=" + Data.ID;
            SPList.Add(new BulkSQL { SQL = Query });

            ReturnValue = await _msBookOracleRepository.ExecMultipleSQLWithTransactionAsync(SPList);

            return ReturnValue;
           
           
        }

        public async Task<ExecuteResult> DeleteBook(msBook Data)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<BulkSQL> SPList = new List<BulkSQL>();
            ModelSQLParamService<msBook> Param = new ModelSQLParamService<msBook>();

            StoredProcedure SP;
            SP = Param.ConvertInSingleLineParam(Data, ParameterType.ExcludeNull);
            string Query = @"UPDATE MSBOOK SET 
                                STSRC='D',
                                USERUP='" + Data.UserUp + @"',
                                DATEUP=(SELECT SYSDATE FROM DUAL)
                            WHERE ID=" + Data.ID;
            SPList.Add(new BulkSQL { SQL = Query });

            ReturnValue = await _msBookOracleRepository.ExecMultipleSQLWithTransactionAsync(SPList);

            return ReturnValue;


        }
    }
}
