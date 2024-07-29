namespace Notifications.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ClientMessage> ClientMessages { get; set; } = null!;
        public DbSet<Visitor> Visitors { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClientMessageMap());
            modelBuilder.ApplyConfiguration(new VisitorMap());
        }
    }
}
