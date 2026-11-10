namespace RecordingGrpcService.Infrastructure.Interfaces;

public interface IBlobObjectStorage
{
    Task<T?> DownloadJsonAsync<T>(string container, string path, CancellationToken cancellationToken) where T : class;
}