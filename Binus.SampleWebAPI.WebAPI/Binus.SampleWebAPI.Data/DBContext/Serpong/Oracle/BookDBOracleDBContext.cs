using System.Data.Entity;

namespace Binus.SampleWebAPI.Data.DBContext.Serpong.Oracle
{
    public class BookDBOracleDBContext : DbContext
    {
        public BookDBOracleDBContext() : base("BookDBOracleDBContext") {
            Database.SetInitializer(new DropCreateDatabaseAlways<BookDBOracleDBContext>());
        }

        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("BOOKDB");
        }
    }

}
