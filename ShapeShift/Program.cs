using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShapeShift.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GymDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
builder.Services.AddIdentity<Trainer, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<GymDbContext>()
    .AddDefaultTokenProviders();

// Configure authentication middleware (optional)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Specify your login path
    options.AccessDeniedPath = "/Account/AccessDenied"; // Specify access denied path
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("RequireTrainerRole", policy =>
        policy.RequireRole("Trainer"));

    // You can define more policies as needed
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDM0MjEzQDMxMzkyZTMxMmUzMGtBR1h5Rk56UkRXbXZ6NUFSN2M0OXc0ZXlaSEZFeGlhQlZDdW9hdDZwQUU9;NDM0MjE0QDMxMzkyZTMxMmUzMFptdlZOck5kQlRoa3JubHhkaFNSVGhjTnd3QUhGeXBBZ2tpb21VbEYxRzQ9;NDM0MjE1QDMxMzkyZTMxMmUzMGFQQloveEM4WEpNSWFYYWE3RUtzVGkyRkhJamwvQ2dtcHhRaXlaVmZUV1k9");



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
     name: "admin",
     pattern: "Admin/{action}/{id?}",
     defaults: new { controller = "Admin", action = "AdminDashboard" }
 );
    endpoints.MapControllerRoute(
     name: "trainerdashboard",
     pattern: "Trainer/{action}/{id?}",
     defaults: new { controller = "Trainer", action = "TrainerDashboard" }
 );
    endpoints.MapControllerRoute(
    name: "members",
    pattern: "Members/{action}/{id?}",
    defaults: new { controller = "Members", action = "MembersDashboard" }
);

    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );
    endpoints.MapControllerRoute(
    name: "trainerlogin",
    pattern: "TrainerAccount/{action}/{id?}",
    defaults: new { controller = "TrainerAccount", action = "TrainerLogin" }
);






});

app.Run();
