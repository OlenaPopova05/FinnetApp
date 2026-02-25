namespace Contracts.Infrastructure.Repositories;

using Contracts.Application;
using Contracts.Domain.Entities;
using Contracts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public sealed class ContractsRepository(ContractsPersistence persistence) : IContractsRepository
{
    public async Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await persistence.Contracts.FindAsync([id], cancellationToken: cancellationToken);

    public async Task<Contract?> GetPreviousForCustomerAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        await persistence.Contracts
            .OrderByDescending(contract => contract.PreparedAt)
            .SingleOrDefaultAsync(contract => contract.CustomerId == customerId, cancellationToken);

    public async Task AddAsync(Contract contract, CancellationToken cancellationToken = default) =>
        await persistence.Contracts.AddAsync(contract, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await persistence.SaveChangesAsync(cancellationToken);
}
