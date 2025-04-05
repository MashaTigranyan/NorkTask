using MariamApp.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MariamApp.Data;

public class AppUsersDbContext : IdentityDbContext<ApplicationUser>
{
    public AppUsersDbContext(DbContextOptions<AppUsersDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Role>().ToTable("AspNetRoles");
    }
}