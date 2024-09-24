using Microsoft.EntityFrameworkCore;
using ProgressTestTask.DAL;
using ProgressTestTask.DAL.Interfaces;
using ProgressTestTask.DAL.Repositories;
using ProgressTestTask.Domain.Entity;
using ProgressTestTask.Service.Implementations;
using ProgressTestTask.Service.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connection = builder.Configuration.GetConnectionString("localDb");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connection));

builder.Services.AddScoped<IBaseRepository<Patient>, PatientRepository>();
builder.Services.AddScoped<IBaseRepository<Visit>, VisitRepository>();
builder.Services.AddScoped<IBaseRepository<MKB10>, MKB10Repository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IMKB10Service, MKB10Service>();


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=Index}/{id?}");

app.Run();
