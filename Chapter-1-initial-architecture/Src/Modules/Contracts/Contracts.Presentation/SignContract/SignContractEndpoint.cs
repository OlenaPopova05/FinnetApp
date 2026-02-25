namespace Contracts.Presentation.SignContract;

using Contracts.Application;
using Contracts.Application.Common;
using Contracts.Application.SignContract;
using Contracts.Domain.Events;
using Contracts.Presentation.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

internal static class SignContractEndpoint
{
    internal static void MapSignContract(this IEndpointRouteBuilder app) => app.MapPatch(ContractsApiPaths.Sign,
            async (Guid id, SignContractRequest request,
                IContractsRepository repository,
                IEventPublisher eventPublisher,
                TimeProvider timeProvider,
                CancellationToken cancellationToken) =>
            {
                var contract = await repository.GetByIdAsync(id, cancellationToken);

                if (contract is null)
                {
                    return Results.NotFound();
                }

                var dateNow = timeProvider.GetUtcNow();
                contract.Sign(request.SignedAt, dateNow);
                await repository.SaveChangesAsync(cancellationToken);

                var @event = ContractSignedEvent.Create(
                    contract.Id,
                    contract.CustomerId,
                    contract.SignedAt!.Value,
                    contract.ExpiringAt!.Value,
                    timeProvider.GetUtcNow());
                await eventPublisher.PublishAsync(@event, cancellationToken);

                return Results.NoContent();
            })
        .ValidateRequest<SignContractRequest>()
        .WithSummary("Signs prepared contract")
        .WithDescription("This endpoint is used to sign prepared contract by customer.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError);
}
