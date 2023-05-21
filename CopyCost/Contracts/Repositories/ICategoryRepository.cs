using CopyCost.Dto;
using CopyCost.Entities;
using CopyCost.Extensions;

namespace CopyCost.Contracts.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OperationResult> AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateAsync(Category category, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> PaymentsExistAsync(int id, CancellationToken cancellationToken = default);
    Task<List<CategoryByText>> GetCategoriesByTextAsync(int customerId, CancellationToken cancellationToken = default);
    Task<List<CategorySummary>> GetCategorySummaryAsync(int categoryId, CancellationToken cancellationToken = default);
}