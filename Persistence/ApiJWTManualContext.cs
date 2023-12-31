using System.Reflection;
using Domain.Entities;
using Domain.Entities.Generics;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class ApiJWTManualContext : DbContext{
    public ApiJWTManualContext(DbContextOptions<ApiJWTManualContext> conf) : base(conf){}

    // DbSets

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshTokenRecord> RefreshTokenRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder){  
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}