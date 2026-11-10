using Microsoft.Extensions.DependencyInjection;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Application.Services;

namespace RecordingGrpcService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRecordingService, RecordingService>();
        services.AddScoped<IRecordingChunkService, RecordingChunkService>();
        services.AddScoped<IRecordingDiarizationService, RecordingDiarizationService>();

        return services;
    }
}