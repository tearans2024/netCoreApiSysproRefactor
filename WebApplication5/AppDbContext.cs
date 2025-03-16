using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Contoh tabel "Users"
    public DbSet<User> Users { get; set; }
}

// Model contoh untuk tabel Users
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
