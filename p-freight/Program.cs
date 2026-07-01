using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using p_freight.Areas.Identity.Data;
using p_freight.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("freightConn") ?? throw new InvalidOperationException("Connection string 'freightConn' not found.");

// Fix for IIS: if relative SQLite path, make it absolute to content root
if (connectionString.Contains("Data Source=") && !connectionString.Contains(":\\") && !connectionString.Contains("/") && !connectionString.Contains("Filename="))
{
    var dataSource = connectionString.Replace("Data Source=", "").Trim();
    if (!Path.IsPathRooted(dataSource))
    {
        var dbPath = Path.Combine(builder.Environment.ContentRootPath, dataSource);
        connectionString = $"Data Source={dbPath}";
    }
}

builder.Services.AddDbContext<p_freightContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDbContext<FreightDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<p_freightUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<p_freightContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Seed roles and default admin user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<p_freightContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try 
    {
        // First, ensure EF core identity standard migrations are applied
        context.Database.Migrate();

        // Find database.sql in the content root (it's copied there during build/publish)
        var sqlPath = Path.Combine(app.Environment.ContentRootPath, "database.sql");
        
        if (File.Exists(sqlPath))
        {
            var sql = File.ReadAllText(sqlPath);
            var statements = sql.Split(';', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var statement in statements)
            {
                if (string.IsNullOrWhiteSpace(statement)) continue;
                try
                {
                    context.Database.ExecuteSqlRaw(statement + ";");
                }
                catch (Exception ex)
                {
                    // Suppress exceptions implicitly providing idempotency
                    logger.LogDebug($"SQL Execution suppression: {ex.Message}");
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during database migration or SQL execution.");
        if (app.Environment.IsDevelopment()) throw; 
    }

    // Seed default Organisation
    var freightDb = scope.ServiceProvider.GetRequiredService<FreightDbContext>();
    const string defaultOrgId = "org-loadlink-default";
    try 
    {
        var defaultOrg = await freightDb.Organisations.FindAsync(defaultOrgId);
        if (defaultOrg == null)
        {
            defaultOrg = new p_freight.Models.Organisation
            {
                Id = defaultOrgId,
                Name = "LoadLink SADC",
                Country = "Zimbabwe",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            freightDb.Organisations.Add(defaultOrg);
            await freightDb.SaveChangesAsync();
        }

        // Seed Roles
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = { "Admin", "Customer", "Driver" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed a default Admin user
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<p_freightUser>>();
        var adminEmail = "admin@freight.co.zw";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new p_freightUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                OrganisationId = defaultOrgId
            };
            var result = await userManager.CreateAsync(admin, "Admin123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
        else
        {
            // Ensure existing admin has org assigned
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin != null && string.IsNullOrEmpty(existingAdmin.OrganisationId))
            {
                existingAdmin.OrganisationId = defaultOrgId;
                await userManager.UpdateAsync(existingAdmin);
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during seeding.");
        if (app.Environment.IsDevelopment()) throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
