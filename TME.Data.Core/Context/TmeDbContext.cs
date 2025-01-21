using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TME.Data.Core.Mappings;
using TME.Domain.Core.Entities;
using TME.Domain.Core.Notifications;


namespace TME.Data.Core.Context
{
    public class TmeDbContext : DbContext
    {
        public TmeDbContext() { }

        public TmeDbContext(DbContextOptions<TmeDbContext> options)
            : base(options)
        { }


        public DbSet<TME_Task> TME_TASK { get; set; }

        public DbSet<TME_Project> TME_PROJECT { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações do Mapeamento das Entidades.
            modelBuilder
                .ApplyConfiguration(new TME_TaskMap())
                .ApplyConfiguration(new TME_ProjectMap())
                .Ignore<DomainNotificationHandler>();

            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Get the configuration from the app settings
                var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string applicationExeDirectory = Path.GetDirectoryName(location);

                var builder = new ConfigurationBuilder()
                    .SetBasePath(applicationExeDirectory)
                    .AddJsonFile("appsettings.json");
                IConfigurationRoot configurationRoot = builder.Build();
                var connectionString = configurationRoot.GetConnectionString("AgriStructureConnection");

                optionsBuilder.UseSqlite(connectionString);
            }
        }
    }
}
