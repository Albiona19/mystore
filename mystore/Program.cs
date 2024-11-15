using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mystore.Models;
using mystore.Services;

var builder = WebApplication.CreateBuilder(args);

// Shto sh�rbimet n� container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Shto dhe konfiguro Identitetin me rolin p�r administratorin
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // Shto mb�shtetje p�r role
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Konfiguro cookie p�r t� ridrejtuar n� faqen e hyrjes
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

// Metoda p�r t� krijuar p�rdoruesin dhe rolin e administratorit
async Task CreateAdminUserAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string adminRole = "Admin";
    string adminEmail = "admin@shembull.com";
    string adminPassword = "Admin123!";

    // Krijo rolin Admin n�se nuk ekziston
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    // Krijo p�rdoruesin administrator n�se nuk ekziston
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(adminUser, adminPassword);
        await userManager.AddToRoleAsync(adminUser, adminRole);
    }
}

// Thirr metod�n p�r t� krijuar administratorin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateAdminUserAsync(services);
}

// Konfiguro pipeline t� k�rkesave HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Shto Autentifikimin
app.UseAuthorization();

app.MapRazorPages();

app.Run();
