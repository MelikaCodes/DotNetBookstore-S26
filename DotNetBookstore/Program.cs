using DotNetBookstore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var postgres = builder.Configuration.GetConnectionString("Postgres");
var hostedOnRender = builder.Configuration["RENDER"] == "true";

if (!string.IsNullOrWhiteSpace(postgres))
{
    builder.Services.AddDbContext<PostgresBookstoreContext>(options => options.UseNpgsql(postgres));
    builder.Services.AddScoped<ApplicationDbContext>(services =>
        services.GetRequiredService<PostgresBookstoreContext>());
}
else if (hostedOnRender)
{
    throw new InvalidOperationException("Set ConnectionStrings__Postgres to the hosted database connection string.");
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    if (hostedOnRender)
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));

if (hostedOnRender)
{
    builder.Services.ConfigureApplicationCookie(options =>
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always);
    builder.Services.AddAntiforgery(options =>
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    if (!hostedOnRender)
        app.UseHsts();
}

// Render terminates TLS and redirects HTTP at its edge.
if (!hostedOnRender)
    app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
}

app.Run();
