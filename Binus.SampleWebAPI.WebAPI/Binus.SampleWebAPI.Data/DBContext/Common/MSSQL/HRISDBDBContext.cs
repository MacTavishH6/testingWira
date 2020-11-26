using Binus.SampleWebAPI.Data.Configurations;
using System.Data.Entity;

namespace Binus.SampleWebAPI.Data.DBContext.Common.MSSQL
{
    [DbConfigurationType(typeof(EntityFrameworkDb2000Configuration))]
    public class HRISDBDBContext : DbContext
    {
        public HRISDBDBContext() : base("HRISDBDBContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // var model = modelBuilder.Build(new System.Data.Entity.Infrastructure.DbProviderInfo("System.Data.SqlClient", "2000"));
            // Compile the model
            //var compiledModel = model.Compile();
            //modelBuilder.Entity<UserHRISDB>().Ignore(e => e.UserActive, );

            // Create the container (DbContext subclass). Ideally all the previous stuff should be cached.
            //return new Entities(connection, compiledModel, true);
        }
    }

}
