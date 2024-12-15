using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SiteWebMultiSport.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Activer la prise en charge des sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SiteWebMultiSport.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Durée de la session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Adherant}/{action=Create}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");


app.MapRazorPages()
   .WithStaticAssets();

// Configuration d'un adhérent administrateur au démarrage
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Vérifie si un administrateur existe déjà
    if (!context.Adherants.Any(a => a.IsAdmin))
    {
        // Ajoute un administrateur par défaut
        var admin = new Adherant
        {
            Name = "Admin",
            DateNaissance = "1990-01-01",
            Email = "admin@example.com",
            Phone = "0123456789",
            IsAdmin = true,
            IsEncadrant = true,
        };
        context.Adherants.Add(admin);
        context.SaveChanges();
    }
}

app.Run();
