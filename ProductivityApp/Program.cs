using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductivityApp.Data.Data;
using ProductivityApp.Data.Data.Repositories;
using ProductivityApp.Models.Models;
using ProductivityApp.Services.Heplers;
using ProductivityApp.Services.Implementations;
using ProductivityApp.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ProductivityApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IAesGcmEncryptionHelper, AesGcmEncryptionHelper>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<IHabitsService, HabitsService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDailyEntryService, DailyEntryService>();
builder.Services.AddScoped<ITaskService, TaskService>();



builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProductivityApp.Web.Mappings.HabitProfile>();
    cfg.AddProfile<ProductivityApp.Web.Mappings.TaskProfile>();
    cfg.AddProfile<ProductivityApp.Web.Mappings.JournalProfile>();
    cfg.AddProfile<ProductivityApp.Web.Mappings.DailyEntryProfile>();

});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
