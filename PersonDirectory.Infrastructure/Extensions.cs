using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonDirectory.Domain.CityManagement.Repositories;
using PersonDirectory.Domain.PersonManagement.ReadServices;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Infrastructure.DataAccess;
using PersonDirectory.Infrastructure.ReadServices;
using PersonDirectory.Infrastructure.Repositories;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PersonDirectoryDbContext");

        services.AddScoped<IDbContext>(x => x.GetService<PersonDirectoryDbContext>()!);

        services.AddDbContext<PersonDirectoryDbContext>((sp, options) =>
            options.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information));

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IPersonReadService, PersonReadService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
