using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcBurger.Areas.Identity.Data;
namespace MvcBurger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("MvcBurgerContextConnection") ?? throw new InvalidOperationException("Connection string 'MvcBurgerContextConnection' not found.");

            builder.Services.AddDbContext<MvcBurgerContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<MvcBurgerUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<MvcBurgerContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
