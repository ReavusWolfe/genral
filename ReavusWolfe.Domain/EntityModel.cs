using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ReavusWolfe.Domain
{
    public class EntityModel : DbContext
    {
        private const string DATABASE_NAME = "Genral";

        public EntityModel() : this($"Data Source=.\\SQLEXPRESS;Initial Catalog={DATABASE_NAME};Integrated Security=True;")
        {
        }

        public EntityModel(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<EntityModel>(null);
            Configuration.LazyLoadingEnabled = false;
            Database.CommandTimeout = 60;
        }

        public virtual DbSet<Model.Money> Money { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
