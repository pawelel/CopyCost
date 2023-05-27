using CopyCost.CCExtensions;
using CopyCost.Entities;

namespace CopyCost.Contracts.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OperationResult> AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(Customer customer, CancellationToken cancellationToken = default);
    Task <bool> PaymentsExistAsync(int id, CancellationToken cancellationToken = default);
}