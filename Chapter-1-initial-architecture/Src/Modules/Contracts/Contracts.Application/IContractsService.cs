namespace Contracts.Application;

public interface IContractsService
{
    Task<Guid> PrepareContractAsync(
        Guid customerId,
        int customerAge,
        int customerHeight,
        DateTimeOffset preparedAt,
        CancellationToken cancellationToken = default);

    Task SignContractAsync(
        Guid contractId,
        DateTimeOffset signedAt,
        CancellationToken cancellationToken = default);
}
