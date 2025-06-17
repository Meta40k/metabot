using metabot.Models;
using Microsoft.EntityFrameworkCore;

namespace metabot;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Contribution> Contributions { get; set; }
    
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<StormSquad> StormSquads { get; set; }

    private readonly string _connectionString;

    public AppDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}