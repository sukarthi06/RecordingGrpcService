using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Grpc.Mappers;
using RecordingGrpcService.Grpc.Protos;
using System.Text.Json;

namespace RecordingGrpcService.Grpc.Services;

public class RecordingsChunkService(
    ILogger<RecordingsChunkService> logger,
    RecordingChunkMapper mapper,
    IRecordingChunkService recordingChunkService) : RecordingChunkService.RecordingChunkServiceBase
{
    public override async Task<Empty> SaveRecordingChunk(RecordingChunkRequest request,
        ServerCallContext context)
    {
        logger.LogInformation("Received SaveRecordingChunk request: {@Request}", JsonSerializer.Serialize(request));
        var entity = mapper.ToDomain(request.RecordingChunk);
        await recordingChunkService.SaveRecordingChunkAsync(entity, context.CancellationToken);
        return new Empty();
    }   
    public override async Task<Empty> UpdateRecordingChunk(UpdateRecordingChunkRequest request,
        ServerCallContext context)
    {
        var chunkId = mapper.MapChunkId(request.ChunkId);
        await recordingChunkService.UpdateStoragePathAsync(chunkId, request.StoragePath, context.CancellationToken);
        return new Empty();
    }
    public override async Task<GetRecordingChunkResponse> GetRecordingChunk(GetRecordingChunkRequest request,
        ServerCallContext context)
    {
        var chunkId = mapper.MapChunkId(request.ChunkId);
        var chunk = await recordingChunkService.GetRecordingChunkAsync(chunkId, context.CancellationToken);
        return chunk is null
            ? new GetRecordingChunkResponse()
            : new GetRecordingChunkResponse { RecordingChunk = mapper.ToDto(chunk!) };
    }
    public override async Task<GetSubsequentRecordingChunksResponse> GetSubsequentRecordingChunks(
        GetSubsequentRecordingChunksRequest request, ServerCallContext context)
    {
        var chunkId = mapper.MapChunkId(request.ChunkId);
        var chunks = await recordingChunkService.GetSubsequentRecordingChunksAsync(chunkId, context.CancellationToken);

        var response = new GetSubsequentRecordingChunksResponse();
        if (chunks is not null)
        {
            response.RecordingChunks.AddRange(mapper.ToDtoList(chunks));
        }
        return response;
    }
    public override async Task<UpdateRecordingChunkWavResponse> UpdateRecordingChunkWav(UpdateRecordingChunkWavRequest request, ServerCallContext context)
    {
        var chunkId = mapper.MapChunkId(request.ChunkId);
        var wavStoragePath = request.WavPath;

        var response = await recordingChunkService.UpdateWavPathAsync(chunkId, wavStoragePath, context.CancellationToken);
        if (response)
            logger.LogInformation("Wav storage path updated for ChunkId: {ChunkId}", chunkId);
        else
            logger.LogInformation("Wav storage path update failed for ChunkId: {ChunkId}", chunkId);

        return new UpdateRecordingChunkWavResponse { IsSuccess = response };
    }
}
