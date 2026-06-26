using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Shop> Shops => Set<Shop>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // For static date, if we are using DateTime.UtcNow, it will cause issues with seeding as the value will change every time we run the application. So we will use a fixed date for seeding.
        var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", CreatedDate = seedDate, IsDeleted = false },
            new Role { Id = 2, Name = "Customer", CreatedDate = seedDate, IsDeleted = false },
            new Role { Id = 3, Name = "Seller", CreatedDate = seedDate, IsDeleted = false },
            new Role { Id = 4, Name = "Dispatcher", CreatedDate = seedDate, IsDeleted = false }
        );
    }
}