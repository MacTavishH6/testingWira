using System.Data.Entity;

namespace Binus.SampleWebAPI.Data.DBContext.Common.MSSQL
{
    //[DbConfigurationType(typeof(EntityFrameworkDb2000Configuration))]
    public class BinusWebAPIDBContext : DbContext
    {
        public BinusWebAPIDBContext() : base("BinusWebAPIDBContext") { }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
