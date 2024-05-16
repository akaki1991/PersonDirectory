using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDirectory.Application.Services;
using PersonDirectory.Shared.Infrastructure.Behaviours;
using System.IO.Abstractions;
using System.Reflection;

namespace PersonDirectory.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IFileSystem, FileSystem>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(IApplication).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(IApplication).Assembly);

        return services;
    }
}
