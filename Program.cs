using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using vipkuryen;
using vipkuryen.Models;
using vipkuryen.Utility.Accounts;

var builder = WebApplication.CreateBuilder(args);

// Veritaban? ba?lant?s?n? yap?land?r?yoruz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 26)))); // MySQL versiyonunu do?ru belirtin

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/admin/login/index";
    options.AccessDeniedPath = "/admin/login/index";
});




//Idenetity Özellik
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false; // Rakam gereksinimi kapal?
    options.Password.RequiredLength = 4;   // Minimum 4 karakter
    options.Password.RequireNonAlphanumeric = false; // Özel karakter gereksinimi kapal?
    options.Password.RequireUppercase = false; // Büyük harf gereksinimi kapal?
    options.Password.RequireLowercase = false; // Küçük harf gereksinimi kapal?
});

// Data Protection (Önemli ve gerekli!)
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "datakeys")));



// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();


// Seed default user (app olu?turulduktan sonra eklenir)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Default admin ve rol eklenmesi için Account.SeedDefaultUser ça?r?l?r
    await Account.SeedDefaultUser(userManager, roleManager);
}


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

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
