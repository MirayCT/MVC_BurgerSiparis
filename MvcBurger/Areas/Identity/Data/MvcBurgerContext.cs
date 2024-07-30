using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcBurger.Areas.Identity.Data;
using MvcBurger.Entities;

namespace MvcBurger.Areas.Identity.Data;

public class MvcBurgerContext : IdentityDbContext<MvcBurgerUser>
{
    public MvcBurgerContext(DbContextOptions<MvcBurgerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    public DbSet<EkstraMalzeme> EkstraMalzemeler {  get; set; }
    public DbSet<Siparis> Siparisler {  get; set; }
    public DbSet<Menu> Menuler {  get; set; }
}
