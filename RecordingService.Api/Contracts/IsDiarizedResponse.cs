namespace RecordingService.Api.Contracts;

public sealed record IsDiarizedResponse(Guid RecordingId, bool IsDiarized);