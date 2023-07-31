using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Obligatorio2023.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ObligatorioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ObligatorioContext") ?? throw new InvalidOperationException("Connection string 'ObligatorioContext' not found.")));

//configurar servicio bd
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    {
        option.LoginPath = "/Sessions/Login";
        option.AccessDeniedPath = "/Sessions/AccesoDenegado";
        option.ExpireTimeSpan= TimeSpan.FromMinutes(20);
    });

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
