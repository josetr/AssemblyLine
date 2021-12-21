using AssemblyLine;
using AssemblyLine.Blazor;
using AssemblyLine.Blazor.Extensions;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IDialogService, DialogService>();
builder.Services.AddScoped<IOperationService, OperationService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<ISnackbar, Snackbar>();
builder.Services.AddViewModels();
builder.Services.AddDbContextFactory<AssemblyLineDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddMudServices();

var app = builder.Build();

var dbContextFactory = app.Services.GetService<IDbContextFactory<AssemblyLineDbContext>>();
if (dbContextFactory != null)
{
    using var context = dbContextFactory.CreateDbContext();
    if (!context.Exists())
        TestData.FillDatabaseWithTestData(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
