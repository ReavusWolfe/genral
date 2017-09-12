using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ReavusWolfe.Domain
{
    public class EntityModel : DbContext
    {
        private const string DATABASE_NAME = "";
        private const string DATABASE_USER = "";
        private const string DATABASE_PASSWORD = "";

        public EntityModel() : this($"data source=dev.totalren.com;initial catalog={DATABASE_NAME};persist security info=True;user id={DATABASE_USER};password={DATABASE_PASSWORD};MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public EntityModel(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<EntityModel>(null);
            Configuration.LazyLoadingEnabled = false;
            Database.CommandTimeout = 60;
        }

        public virtual DbSet<FooEntity> FooEntities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class FooEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
    }
}
