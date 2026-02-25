namespace Contracts.Application.SignContract;

using FluentValidation;

public sealed class SignContractRequestValidator : AbstractValidator<SignContractRequest>
{
    public SignContractRequestValidator() => RuleFor(request => request.SignedAt).NotEmpty();
}
