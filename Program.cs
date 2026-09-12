using CanteenWeb.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);


// =====================================================
// DATABASE
// =====================================================

var databaseUrl =
    Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Running on Render - PostgreSQL

    var databaseUri = new Uri(databaseUrl);

    var userInfo =
        databaseUri.UserInfo.Split(':');

    var builderConnection =
        new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,

            // Use default PostgreSQL port if no port is specified
            Port = databaseUri.IsDefaultPort
                ? 5432
                : databaseUri.Port,

            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.AbsolutePath.TrimStart('/'),

            SslMode = SslMode.Require
        };

    builder.Services.AddDbContext<CanteenDbContext>(
        options =>
            options.UseNpgsql(
                builderConnection.ConnectionString
            )
    );

    Console.WriteLine("--------------------------------");
    Console.WriteLine("Using PostgreSQL database");
    Console.WriteLine("--------------------------------");
}
else
{
    // Running locally - SQLite

    var databasePath = Path.Combine(
        builder.Environment.ContentRootPath,
        "canteen.db"
    );

    var connectionString =
        $"Data Source={databasePath}";

    builder.Services.AddDbContext<CanteenDbContext>(
        options =>
            options.UseSqlite(connectionString)
    );

    Console.WriteLine("--------------------------------");
    Console.WriteLine(
        $"Using SQLite database: {databasePath}"
    );
    Console.WriteLine("--------------------------------");
}


// =====================================================
// MVC
// =====================================================

builder.Services.AddControllersWithViews();


// =====================================================
// SESSION
// =====================================================

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout =
        TimeSpan.FromMinutes(60);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});


// =====================================================
// BUILD
// =====================================================

var app = builder.Build();


// =====================================================
// DATABASE INITIALIZATION
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<CanteenDbContext>();

    try
    {
        await DbInitializer.InitializeAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("DATABASE ERROR");
        Console.WriteLine(ex);
    }
}


// =====================================================
// HTTP PIPELINE
// =====================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


// Static files
app.UseStaticFiles();

app.UseRouting();


// Session must come before endpoints
app.UseSession();

app.UseAuthorization();


// =====================================================
// DEFAULT ROUTE
// =====================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


Console.WriteLine("--------------------------------");
Console.WriteLine("APPLICATION STARTING");
Console.WriteLine("--------------------------------");


app.Run();