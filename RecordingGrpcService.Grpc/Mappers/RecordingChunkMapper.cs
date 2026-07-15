using Riok.Mapperly.Abstractions;
using RecordingGrpcService.Domain.Entities;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingGrpcService.Grpc.Protos;

namespace RecordingGrpcService.Grpc.Mappers;

[Mapper]
public partial class RecordingChunkMapper : MapperBase
{
    // ---- RecordingChunk <-> RecordingChunkDto ----
    [MapperIgnoreTarget(nameof(RecordingChunk.CreatedAt))]
    [MapperIgnoreTarget(nameof(RecordingChunk.CreatedBy))]
    [MapperIgnoreTarget(nameof(RecordingChunk.LastModifiedAt))]
    [MapperIgnoreTarget(nameof(RecordingChunk.LastModifiedBy))]
    [MapperIgnoreTarget(nameof(RecordingChunk.RecordStatus))]
    [MapperIgnoreSource(nameof(RecordingChunkDto.HasWavPath))]
    public partial RecordingChunk ToDomain(RecordingChunkDto dto);
    [MapperIgnoreSource(nameof(RecordingChunk.CreatedAt))]
    [MapperIgnoreSource(nameof(RecordingChunk.CreatedBy))]
    [MapperIgnoreSource(nameof(RecordingChunk.LastModifiedAt))]
    [MapperIgnoreSource(nameof(RecordingChunk.LastModifiedBy))]
    [MapperIgnoreSource(nameof(RecordingChunk.RecordStatus))]
    public partial RecordingChunkDto ToDto(RecordingChunk entity);

    // ---- List mapping (used for GetRecordingChunkResponse's repeated field) ----
    public partial List<RecordingChunk> ToDomainList(IEnumerable<RecordingChunkDto> dtos);
    public partial List<RecordingChunkDto> ToDtoList(IEnumerable<RecordingChunk> entities);

    // ---- ChunkId (string <-> value object) ----
    public ChunkId MapChunkId(string id) => ChunkId.Of(ParseGuid(id));
    private string MapChunkId(ChunkId id) => id.Value.ToString();

    // ---- RecordingId (string <-> value object) ----
    private RecordingId MapRecordingId(string id) => RecordingId.Of(ParseGuid(id));
    private string MapRecordingId(RecordingId id) => id.Value.ToString();
}