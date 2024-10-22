using AuctionApp.Areas.Identity.Data;
using AuctionApp.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuctionApp.Data;
using AuctionApp.Persistence;
using ProjectApp.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//identity
builder.Services.AddDbContext<AppIdentityDbContext>(options => 
    options.UseMySQL(builder.Configuration.GetConnectionString("AppIdentityDbContextConnection")));
builder.Services.AddDefaultIdentity<AppIdentityUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddDbContext<AuctionDbContext>(
    options => options.UseMySQL(builder.Configuration.GetConnectionString("AuctionDbConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAuctionService, AuctionService>();

// dependency injection of persistence into service
builder.Services.AddScoped<IAuctionPersistence, MySqlAuctionPersistence>();

// auto mapping of data
builder.Services.AddAutoMapper(typeof(Program));

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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();