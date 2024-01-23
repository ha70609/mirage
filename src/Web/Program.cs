using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MirageIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MirageIdentityDbContextConnection' not found.");

// If using SqlServer
// 1.Add SqlServer Pachage
// ```
// ex:
// dotnet add src/Web package Microsoft.EntityFrameworkCore.SqlServer
// ```
// 2.Add using
// ```
// ex:
// Microsoft.EntityFrameworkCore.SqlServer
// ```
// 3. UseSqlServer
// builder.Services.AddDbContext<MirageIdentityDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<MirageIdentityDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MirageIdentityDbContext>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
