using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using p_freight.Areas.Identity.Data;

namespace p_freight.Data;

public class p_freightContext : IdentityDbContext<p_freightUser>
{
    public p_freightContext(DbContextOptions<p_freightContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<p_freightUser>(entity => { 
            entity.ToTable("users"); 
            entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");
        });
        builder.Entity<IdentityRole>(entity => { entity.ToTable("roles"); });
        builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("user_roles"); });
        builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("user_claims"); });
        builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("user_logins"); });
        builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("role_claims"); });
        builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("user_tokens"); });
    }
}
