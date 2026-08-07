using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecordingGrpcService.Infrastructure.Extensions;
using RecordingGrpcService.Infrastructure.Interfaces;
using RecordingGrpcService.Infrastructure.Services;
using Serilog;

namespace RecordingGrpcService.Infrastructure;

public static class DependencyInjection
{
    public static async Task<IServiceCollection> AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)    
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("RecordingDb"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                });

            if (configuration.GetValue<bool>("Logging:LogEfSql"))
            {
                options
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IRecordingRepo, RecordingRepo>();
        services.AddScoped<IRecordingChunkRepo, RecordingChunkRepo>();

        return services;
    }

    public static void AddHostInfrastructure(
        this IHostBuilder hostBuilder,
        IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    }
}
