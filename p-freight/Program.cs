using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using p_freight.Areas.Identity.Data;
using p_freight.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("p_freightContextConnection") ?? throw new InvalidOperationException("Connection string 'p_freightContextConnection' not found.");

builder.Services.AddDbContext<p_freightContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<p_freightUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<p_freightContext>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
