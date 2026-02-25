namespace Contracts.Application;

using Contracts.Application.Common;
using Contracts.Domain.Entities;
using Contracts.Domain.Events;

public sealed class ContractsService(
    IContractsRepository repository,
    IEventPublisher eventPublisher,
    TimeProvider timeProvider) : IContractsService
{
    public async Task<Guid> PrepareContractAsync(
        Guid customerId,
        int customerAge,
        int customerHeight,
        DateTimeOffset preparedAt,
        CancellationToken cancellationToken = default)
    {
        var previousContract = await repository.GetPreviousForCustomerAsync(customerId, cancellationToken);

        var contract = Contract.Prepare(
            customerId,
            customerAge,
            customerHeight,
            preparedAt,
            previousContract?.Signed);

        await repository.AddAsync(contract, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return contract.Id;
    }

    public async Task SignContractAsync(
        Guid contractId,
        DateTimeOffset signedAt,
        CancellationToken cancellationToken = default)
    {
        var contract = await repository.GetByIdAsync(contractId, cancellationToken)
            ?? throw new InvalidOperationException($"Contract with ID {contractId} not found");

        var dateNow = timeProvider.GetUtcNow();
        contract.Sign(signedAt, dateNow);
        await repository.SaveChangesAsync(cancellationToken);

        var @event = ContractSignedEvent.Create(
            contract.Id,
            contract.CustomerId,
            contract.SignedAt!.Value,
            contract.ExpiringAt!.Value,
            timeProvider.GetUtcNow());

        await eventPublisher.PublishAsync(@event, cancellationToken);
    }
}
