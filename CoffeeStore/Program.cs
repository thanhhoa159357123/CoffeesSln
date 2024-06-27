using CoffeeStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CoffeeDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeStoreConnection"));
});

builder.Services.AddScoped<ICoffeeRepository, EFCoffeeRepository>();
builder.Services.AddScoped<ICustomerRepository, EFCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddRazorPages();

// Add session and distributed memory cache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Register HttpContextAccessor and Cart service
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

// Add authentication and authorization services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // Set the login path
        options.LogoutPath = "/Login/Logout"; // Set the logout path
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLoggedIn", policy =>
        policy.RequireAuthenticatedUser());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true, // Serve unknown file types
    DefaultContentType = "image/png" // Default content type for unknown file types
});

app.UseSession(); // Use session middleware

app.UseRouting(); // Use routing middleware

app.UseAuthentication(); // Use authentication middleware
app.UseAuthorization(); // Use authorization middleware

app.MapDefaultControllerRoute();
app.MapRazorPages();

// Ensure database is populated with initial data
CoffeeData.EnsurePopulated(app);

app.Run();
