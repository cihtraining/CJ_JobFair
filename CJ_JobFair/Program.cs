using CJ_JobFair;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddTransient<MySqlDatabase>(_ => new MySqlDatabase("Server=mysql-201ca13b-cihfullstackdotnet-4da2.e.aivencloud.com;User ID=avnadmin;Password=AVNS_N6L8JgMQVB4SIrKWYSE;Database=defaultdb;"));
builder.Services.AddTransient<MySqlDatabase>(_ => new MySqlDatabase("Server=mysql-201ca13b-cihfullstackdotnet-4da2.e.aivencloud.com;Port=18501;User ID=avnadmin;Password=AVNS_N6L8JgMQVB4SIrKWYSE;Database=defaultdb"));
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Add session services with options
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
    options.Cookie.HttpOnly = true; // Make session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make session cookie essential for the application
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/LoginView";
        options.LogoutPath = "/Login/Logout";
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
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Adminroot")),
//    RequestPath = "~/Adminroot"
//});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Areas", "Admin")),
    RequestPath = "/Adminroot"
});

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Login}/{action=LoginView}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registration}/{action=RegistrationForm}/{id?}");

app.Run();
