using Microsoft.EntityFrameworkCore;
using OperatorApp.Core.Entities;
using OperatorApp.Core.Interfaces;

namespace OperatorApp.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Operator> Operators { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operator>()
            .HasKey(o => o.Code);

        modelBuilder.Entity<Operator>()
            .HasIndex(o => o.Name)
            .IsUnique();
    }
}