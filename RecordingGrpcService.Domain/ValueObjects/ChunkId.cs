namespace RecordingGrpcService.Domain.ValueObjects;

public record ChunkId
{
    public Guid Value { get; }
    private ChunkId(Guid value) => Value = value;

    public static ChunkId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("ChunkId cannot be empty.", nameof(value));
        }
        return new ChunkId(value);
    }
    public static bool Empty(Guid value)
    {
        return value == Guid.Empty;
    }
}
