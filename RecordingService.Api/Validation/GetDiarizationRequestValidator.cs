using FluentValidation;
using RecordingService.Api.Contracts;

namespace RecordingService.Api.Validation;

public sealed class GetDiarizationRequestValidator : AbstractValidator<GetDiarizationRequest>
{
    public GetDiarizationRequestValidator()
    {
        RuleFor(x => x.RecordingId)
            .NotEmpty()
            .WithMessage("RecordingId cannot be empty.");
    }
}