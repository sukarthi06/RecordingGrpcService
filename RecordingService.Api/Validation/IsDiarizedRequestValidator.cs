using FluentValidation;
using RecordingService.Api.Contracts;

namespace RecordingService.Api.Validation;

public sealed class IsDiarizedRequestValidator : AbstractValidator<IsDiarizedRequest>
{
    public IsDiarizedRequestValidator()
    {
        RuleFor(x => x.RecordingId)
            .NotEmpty()
            .WithMessage("RecordingId cannot be empty.");
    }
}