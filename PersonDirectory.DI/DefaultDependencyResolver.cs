using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDirectory.Application;
using PersonDirectory.Infrastructure;
using PersonDirectory.Shared.Infrastructure;
using PersonDirectory.Shared.Infrastructure.Abstractios;
using PersonDirectory.Shared.Infrastructure.Behaviours;

namespace PersonDirectory.DI;


public class DefaultDependencyResolver(IConfiguration configurationRoot)
{
    private readonly IConfiguration _configurationRoot = configurationRoot;

    public IServiceCollection RegisterServices(IServiceCollection services)
    {
        services ??= new ServiceCollection();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(IApplication).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

        services.AddInfrastructure(_configurationRoot);
        services.AddApplication(_configurationRoot);


        return services;
    }
}
