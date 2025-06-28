using Encryption_service.Bootstrapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCustomSetup();

builder.Services.AddSwaggerGen(config => config.IncludeXmlComments(Path.Combine(
    AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")));

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHsts();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "/");

app.Run();
