using ProjektGrupowyGis.
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ProjektGrupowyGis.DAL
{
    public class AccountContext : DbContext
    {

        public AccountContext() : base("AccountContext")
        {
        }

        public DbSet<User> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}