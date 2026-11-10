namespace RecordingGrpcService.Infrastructure.Options;

public sealed class AzureBlobStorageOptions
{
    public const string SectionName = "Azure";
    public required string TranscriptsContainer { get; init; }
}