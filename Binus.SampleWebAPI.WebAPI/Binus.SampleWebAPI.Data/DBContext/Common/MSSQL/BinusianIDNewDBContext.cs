using System.Data.Entity;
using Binus.WebAPI.Model.Common;
using Binus.SampleWebAPI.Data.Configurations;

namespace Binus.SampleWebAPI.Data.DBContext.Common.MSSQL
{
    [DbConfigurationType(typeof(EntityFrameworkDb2000Configuration))]
    public class BinusianIDNewDBContext : DbContext
    {
        public BinusianIDNewDBContext() : base("BinusianIDNewDBContext") { }

        public DbSet<UserHRISDB> UsersHRISDB { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo("System.Data.SqlClient", "2000"));
        }
    }
}
