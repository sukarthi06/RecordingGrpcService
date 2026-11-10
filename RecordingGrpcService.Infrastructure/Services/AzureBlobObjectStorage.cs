using System.Text.Json;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using RecordingGrpcService.Infrastructure.Interfaces;

namespace RecordingGrpcService.Infrastructure.Services;

public sealed class AzureBlobObjectStorage(
    BlobServiceClient blobServiceClient,
    ILogger<AzureBlobObjectStorage> logger) : IBlobObjectStorage
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = null,
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> DownloadJsonAsync<T>(string container, string path, CancellationToken cancellationToken) where T : class
    {
        var blobClient = blobServiceClient
            .GetBlobContainerClient(container)
            .GetBlobClient(path);

        try
        {
            if (!await blobClient.ExistsAsync(cancellationToken))
            {
                logger.LogWarning("Blob not found. Container: {Container}, Path: {Path}", container, path);
                return null;
            }

            var response = await blobClient.DownloadContentAsync(cancellationToken);
            return response.Value.Content.ToObjectFromJson<T>(JsonOptions);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize blob {Container}/{Path} as {Type}", container, path, typeof(T).Name);
            return null;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Failed to download blob {Container}/{Path}", container, path);
            return null;
        }
    }
}