using Microsoft.AspNetCore.Http.HttpResults;
using RecordingGrpcService.Application.RepoServices;
using RecordingGrpcService.Domain.ValueObjects;
using RecordingService.Api.Contracts;
using RecordingService.Api.Mappers;
using RecordingService.Api.Validation;

namespace RecordingService.Api.Endpoints;

public static class RecordingEndpoints
{
    public static IEndpointRouteBuilder MapRecordingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/recordings").WithTags("Recordings");

        group.MapGet("/{recordingId:guid}/diarized", IsDiarizedAsync)
            .WithName("IsDiarized")
            .WithSummary("Checks whether a recording has been diarized.")
            .Produces<IsDiarizedResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithValidation<IsDiarizedRequest>();

        group.MapGet("/{recordingId:guid}/diarization", GetDiarizationAsync)
            .WithName("GetDiarization")
            .WithSummary("Gets the consolidated speaker diarization for a recording.")
            .Produces<IReadOnlyList<ConsolidatedSpeakerSegmentsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithValidation<GetDiarizationRequest>();

        return app;
    }

    private static async Task<Ok<IsDiarizedResponse>> IsDiarizedAsync(
        [AsParameters] IsDiarizedRequest request,
        IRecordingService recordingService,
        CancellationToken cancellationToken)
    {
        var isDiarized = await recordingService.IsDiarizedAsync(RecordingId.Of(request.RecordingId), cancellationToken);
        return TypedResults.Ok(new IsDiarizedResponse(request.RecordingId, isDiarized));
    }

    private static async Task<Results<Ok<IReadOnlyList<ConsolidatedSpeakerSegmentsResponse>>, NotFound>> GetDiarizationAsync(
        [AsParameters] GetDiarizationRequest request,
        IRecordingDiarizationService diarizationService,
        CancellationToken cancellationToken)
    {
        var segments = await diarizationService.GetConsolidatedDiarizationAsync(
            RecordingId.Of(request.RecordingId), cancellationToken);

        return segments is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(segments.ToResponse());
    }
}