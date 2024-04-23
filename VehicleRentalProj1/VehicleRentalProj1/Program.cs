using Microsoft.EntityFrameworkCore;
using VehicleRentalProj1.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajoutez les services au conteneur.
builder.Services.AddControllersWithViews();
var connectionstring = builder.Configuration.GetConnectionString("con");
builder.Services.AddDbContext<VehiclesRent1Context>(options => options.UseSqlServer(connectionstring));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});

var app = builder.Build();

// Configurez le pipeline de requête HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // La valeur HSTS par défaut est de 30 jours. Vous voudrez peut-être la modifier pour les scénarios de production, voir https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Ajoutez l'utilisation de la session avant d'autres middlewares de routage ou d'authentification
app.UseSession();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
