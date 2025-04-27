using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using TaskManager.AppDbContext;
using TaskManager.Interface;
using TaskManager.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter()); // Requires authentication for all controllers
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services (Dependency Injection)
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Enable session management
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session expires after 30 minutes
    options.Cookie.HttpOnly = true; // Ensures session cookies are not accessible via JavaScript
    options.Cookie.IsEssential = true; // Ensures session remains active
});

// Configure authentication with cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/ValidateUser"; // Redirects to login if not authenticated
        options.AccessDeniedPath = "/Home/AccessDenied"; // Redirects unauthorized users
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Authentication middleware
app.UseAuthorization(); // Authorization middleware
app.UseSession(); // Session middleware (Make sure this is added!)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
