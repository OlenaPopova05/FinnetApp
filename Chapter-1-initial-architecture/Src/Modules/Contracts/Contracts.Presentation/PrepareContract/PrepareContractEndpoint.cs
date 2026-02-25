namespace Contracts.Presentation.PrepareContract;

using Contracts.Application;
using Contracts.Application.PrepareContract;
using Contracts.Domain.Entities;
using Contracts.Presentation.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

internal static class PrepareContractEndpoint
{
    internal static void MapPrepareContract(this IEndpointRouteBuilder app) =>
        app.MapPost(ContractsApiPaths.Prepare,
                async (PrepareContractRequest request,
                    IContractsRepository repository,
                    CancellationToken cancellationToken) =>
                {
                    var previousContract =
                        await repository.GetPreviousForCustomerAsync(request.CustomerId, cancellationToken);
                    var contract = Contract.Prepare(
                        request.CustomerId,
                        request.CustomerAge,
                        request.CustomerHeight,
                        request.PreparedAt,
                        previousContract?.Signed);
                    await repository.AddAsync(contract, cancellationToken);
                    await repository.SaveChangesAsync(cancellationToken);

                    return Results.Created($"/{ContractsApiPaths.Prepare}/{contract.Id}", contract.Id);
                })
            .ValidateRequest<PrepareContractRequest>()
            .WithSummary("Triggers preparation of a new contract for new or existing customer")
            .WithDescription("This endpoint is used to prepare a new contract for new and existing customers.")
            .Produces<string>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);
}
