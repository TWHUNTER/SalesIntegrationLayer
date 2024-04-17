namespace IntegracionDesarrollo3
{
    using IntegracionDesarrollo3.Models;
    using Microsoft.EntityFrameworkCore;

    public class IntegrationDatabase : DbContext
    {
        public IntegrationDatabase(DbContextOptions<IntegrationDatabase> opts) : base(opts)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<ServicesModel> services { get; set; }
        public DbSet<Category> products_category { get; set; }
        public DbSet<Invoices> invoices { get; set; }
        public DbSet<Profile> profiles { get; set; }

    }
}
