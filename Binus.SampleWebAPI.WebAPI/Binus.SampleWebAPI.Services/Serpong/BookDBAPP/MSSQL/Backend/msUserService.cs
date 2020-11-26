using System.Collections.Generic;
using System.Threading.Tasks;
using Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MSSQL.Backend;
using Binus.SampleWebAPI.Data.Repositories.Serpong.MSSQL.Backend;
using Binus.WebAPI.Model.MSSQL;
using Binus.WebAPI.MSSQL;
using System.Data.SqlClient;
using Binus.WebAPI.Cryptography;

namespace Binus.SampleWebAPI.Services.Serpong.BookDBAPP.MSSQL.Backend
{
    public interface ImsUserService
    {
        Task<msUser> GetOne(msUser User);



    }
    public class msUserService : ImsUserService
    {

        private readonly ImsUserRepository _msUserRepository;



        public msUserService(ImsUserRepository msUserRepository)
        {
           // RolesRepository.ConnectionString("DeveloperCompetencyDBEntities");
            this._msUserRepository = msUserRepository;
        }

        public async Task<msUser> GetOne(msUser User)
        {
            var Param = new[] { new SqlParameter("@Username", Crypto.Decrypt(User.Username)), new SqlParameter("@Password", Crypto.Decrypt(User.Password)) };
            return await _msUserRepository.ExecSPToSingleAsync("bn_BookDB_GetUser @Username,@Password", Param);
        }

     
    }
}
