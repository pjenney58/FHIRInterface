using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PalisaidPod.Models.Server;

namespace PalisaidPod.Data;

// <summary>
/// ApplicationDbContext - Supports Azure Identity Management in our PostgreSQL db </summary>
public class ApplicationDbContext : IdentityDbContext<TenantUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        base.OnConfiguring(options);
    }
}