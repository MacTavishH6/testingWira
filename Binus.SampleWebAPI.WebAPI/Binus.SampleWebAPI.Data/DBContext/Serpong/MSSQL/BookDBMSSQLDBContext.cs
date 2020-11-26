using System.Data.Entity;

namespace Binus.SampleWebAPI.Data.DBContext.Serpong.MSSQL
{
    //[DbConfigurationType(typeof(EntityFrameworkDb2000Configuration))]
    public class BookDBMSSQLDBContext : DbContext
    {
        public BookDBMSSQLDBContext() : base("BookDBMSSQLDBContext") { }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
