using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Binus.WebAPI.Model.Common;
using System;
using Binus.SampleWebAPI.Data.Repositories.Common.MSSQL;

namespace Binus.SampleWebAPI.Services.Common
{
    public interface IUserHRISDBService
    {

        IEnumerable<UserHRISDB> GetUserHRISByEmail(string Email);
        Task<IEnumerable<UserHRISDB>> GetUserHRISByEmailAsync(string Email);
        Task<UserHRISDB> GetEmployeeEmailAsync(string BinusianID);
        Task<EmployeeDB> GetEmployeeDataByEmailOrBinusianIDAsync(string BinusianID="", string Email = "");
        IEnumerable<UserHRISDB> GetSubordinate(string BinusianID);
        Task<IEnumerable<UserHRISDB>> GetSubordinateAsync(string BinusianID);
        Task<IEnumerable<UserHRISDB>> GetSupervisorAsync(string BinusianID, string BandID);
        BinusianPhoto GetBinusianPicture(string BinusianID);
        Task<IEnumerable<UserHRISDB>> GetUserGroupAsync(int GroupID = 0, int SubGroupID = 0);
        UserHRISDB GetUserGroupbyEmail(string Email);
        Task<IEnumerable<UserHRISDB>> GetHodHopData(string BinusianID = "");
        Task<IEnumerable<UserHRISDB>> GetHopByKDJabatan(string KDJabatan = "");
        UserHRISDB GetUserGroupbyBinusianID(string BinusianID);
        IEnumerable<UserHRISDB> GetUserHRISByEmailSP(string Email);
        Task<IEnumerable<UserHRISDB>> GetEmployeeHRISByKeyAsync(string Key);
    }
    public class UserHRISDBService : IUserHRISDBService
    {
       
        private readonly IUserHRISDBRepository UserHRISDBRepository;
        private readonly IBinusianPhotoRepository BinusianPhotoRepository;
        private readonly IEmployeeRepository EmployeeRepository;
        // private readonly IUnitOfWork UnitOfWork;

        public UserHRISDBService(IUserHRISDBRepository UserHRISDBRepository, IBinusianPhotoRepository BinusianPhotoRepository, IEmployeeRepository EmployeeRepository)
        {
            
            this.UserHRISDBRepository = UserHRISDBRepository;
            this.BinusianPhotoRepository = BinusianPhotoRepository;
            this.EmployeeRepository = EmployeeRepository;
            //this.UnitOfWork = UnitOfWork;
        }

        public async Task<EmployeeDB> GetEmployeeDataByEmailOrBinusianIDAsync(string BinusianID = "", string Email = "")
        {
            var Param = new[] { new SqlParameter("@Email", Email), new SqlParameter("@BinusianID", BinusianID) };
          
                return await EmployeeRepository.ExecSPToSingleAsync("bn_HRISDB_GetEmployeeDataByEmailOrBinusianIDSampleWEBAPI @Email,@BinusianID", Param);
          

        }
        public async Task<IEnumerable<UserHRISDB>> GetEmployeeHRISByKeyAsync(string Key)
        {
            //var Param = new[] { new SqlParameter("@Key", Key.Trim()) };
            try
            {
                return await UserHRISDBRepository.ExecSPToListAsync(@"SELECT a.binusian_id, a.nama, b.kd_jabatan, c.nm_jabatan, a.email1 as Email FROM master_data_pribadi_pegawai a INNER JOIN transaksi_jabatan_pegawai b ON a.binusian_id = b.binusian_id INNER JOIN tabel_kode_jabatan c ON c.kd_jabatan = b.kd_jabatan WHERE a.Binusian_ID LIKE '%" + Key + "%' OR Nama LIKE '%" + Key + "%' ORDER BY nama ");
            }
            catch(Exception EX)
            {
                return null;
            }
          
        }


        public IEnumerable<UserHRISDB> GetUserHRISByEmail(string Email)
        {
            var Param = new[] { new SqlParameter("@Email", Email) };
            return UserHRISDBRepository.ExecSPToList("bn_HRISDB_GetUserOrganization @Email", Param);
           // return UserHRISDBRepository.ExecSPToList("SELECT * FROM vpr_Load_Struktur_Organisasi").Where(c => c.Email1 ==  Email);

        }

        public IEnumerable<UserHRISDB> GetUserHRISByEmailSP(string Email)
        {
            return UserHRISDBRepository.ExecSPToList("SELECT * FROM vpr_Load_Struktur_Organisasi WHERE Email1 ='"+ Email + "'");

        }

        public async Task<IEnumerable<UserHRISDB>> GetUserHRISByEmailAsync(string Email)
        {
            var Param = new[] { new SqlParameter("@Email", Email) };
            return await UserHRISDBRepository.ExecSPToListAsync("bn_HRISDB_GetUserOrganization @Email", Param);
        }

    
        public async Task<IEnumerable<UserHRISDB>> GetHodHopData(string BinusianID = "")
        {
            var Param = new[] { new SqlParameter("@BinusianID", BinusianID) };
            return await UserHRISDBRepository.ExecSPToListAsync("bn_HRISDB_GetHodHopData @BinusianID", Param);
        }

        public async Task<IEnumerable<UserHRISDB>> GetHopByKDJabatan(string KDJabatan = "")
        {
            var Param = new[] { new SqlParameter("@KDJabatan", KDJabatan) };
            return await UserHRISDBRepository.ExecSPToListAsync("bn_HRISDB_GetHOPByKDJabatan @KDJabatan", Param);
        }

        public async Task<UserHRISDB> GetEmployeeEmailAsync(string BinusianID)
        {
            var Param = new[] { new SqlParameter("@BinusianID", BinusianID) };
            return await UserHRISDBRepository.ExecSPToSingleAsync("bn_HRISDB_GetEmployeeEmail @BinusianID", Param);
        }

        public IEnumerable<UserHRISDB> GetSubordinate(string BinusianID)
        {
            var Param = new[] { new SqlParameter("@BinusianID", BinusianID.Trim()) };
            return UserHRISDBRepository.ExecSPToList("bn_HRISDB_GetSubordinate @BinusianID", Param).Where(x => x.Binusian_ID.Trim()  != BinusianID.Trim());
            // return UserHRISDBRepository.ExecSPToList("SELECT * FROM vpr_Load_Struktur_Organisasi").Where(c => c.Email1 ==  Email);

        }

        public async Task<IEnumerable<UserHRISDB>> GetSupervisorAsync(string BinusianID, string BandID)
        {
            var Param = new[] { new SqlParameter("@BinusianID", BinusianID.Trim()) };
            if (BandID == "00")
            {
                return (await UserHRISDBRepository.ExecSPToListAsync("bn_HRISDB_GetDirectSupervisor @BinusianID", Param));
            }
            else
            {
                return (await UserHRISDBRepository.ExecSPToListAsync("bn_HRISDB_GetDirectSupervisor @BinusianID", Param)).Where(X => X.BandID.Trim() == BandID);
            }
        }

        public async Task<IEnumerable<UserHRISDB>> GetSubordinateAsync(string BinusianID)
        {
            var Param = new[] { new SqlParameter("@BinusianID", BinusianID.Trim()) };
            return await UserHRISDBRepository.ExecSPToListAsync("[bn_HRISDB_GetInfoStaffBySupervisorAndRequestID] @BinusianID", Param);
        }

        public BinusianPhoto GetBinusianPicture(string BinusianID)
        {
            //var Param = new[] { new SqlParameter("@BinusianID", BinusianID) };
             //return BinusianPhotoRepository.ExecSPToSingle("SELECT [photo] FROM [tabel_photo] WHERE [binusian_id]='" + BinusianID+"'");   
            return BinusianPhotoRepository.ExecSPToSingle("[dbo].[getPhoto] @BinusianID='"+BinusianID+"'");
        }

        public async Task<IEnumerable<UserHRISDB>> GetUserGroupAsync(int GroupID = 0, int SubGroupID = 0)
        {
            var Param = new[] { new SqlParameter("@GroupID", GroupID), new SqlParameter("@SubGroupID", SubGroupID) };
            return await UserHRISDBRepository.ExecSPToListAsync("bn_Group_GetGroupUser @GroupID, @SubGroupID", Param);
        }

        public UserHRISDB GetUserGroupbyEmail(string Email) {
            var Param = new[] { new SqlParameter("@Email", Email) };
            return UserHRISDBRepository.ExecSPToSingle("bn_Group_GetGroupUserByEmail @Email", Param);
        }

        public UserHRISDB GetUserGroupbyBinusianID(string BinusianID)
        {
            var Param = new[] { new SqlParameter("@BinusianID", BinusianID) };
            return UserHRISDBRepository.ExecSPToSingle("bn_Group_GetGroupUserByBinusianID @BinusianID", Param);
        }

    }
}