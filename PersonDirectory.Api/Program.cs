using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PersonDirectory.Api.Filters;
using PersonDirectory.Api.Middlewares;
using PersonDirectory.DI;
using PersonDirectory.Domain.CityManagement;
using PersonDirectory.Infrastructure.DataAccess;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
new DefaultDependencyResolver(builder.Configuration).RegisterServices(builder.Services);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Person Directory", Version = "v1" });
    c.OperationFilter<CustomHeaderOperationFilter>();
    c.EnableAnnotations();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.File(Path.Combine(Environment.CurrentDirectory, $"Files/Logs/Log.txt"))
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LocalizationMiddleware>();

await InitializeDatabaseAsync(app);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();


static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<PersonDirectoryDbContext>();

    await context.Database.MigrateAsync();

    await SeedCitiesAsync(context);
}

static async Task SeedCitiesAsync(PersonDirectoryDbContext context)
{
    if (!await context.Cities.AnyAsync())
    {
        await context.Cities.AddRangeAsync(new List<City>
        {
            City.With("Tbilisi"),
            City.With("Batumi"),
            City.With("Martvili"),
        });
        await context.SaveChangesAsync();
    }
}