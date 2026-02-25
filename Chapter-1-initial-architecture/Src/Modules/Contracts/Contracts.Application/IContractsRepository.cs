namespace Contracts.Application;

using Contracts.Domain.Entities;

public interface IContractsRepository
{
    Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Contract?> GetPreviousForCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task AddAsync(Contract contract, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
