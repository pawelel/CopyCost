using System.Globalization;
using CopyCost.CCExtensions;
using CopyCost.Contracts.Repositories;
using CopyCost.Data;
using CopyCost.Dto;
using CopyCost.Entities;

using Microsoft.EntityFrameworkCore;

namespace CopyCost.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbContextFactory<CopyCostDbContext> _dbContextFactory;

    public CategoryRepository(IDbContextFactory<CopyCostDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Category?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Categories.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return context.Categories.AsEnumerable()
            .OrderBy(c => c.Name, StringComparer.Create(new CultureInfo("pl-PL"), false))
            .ToList();
    }

    public async Task<OperationResult> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        if (await IsCategoryNameTaken(category.Name, cancellationToken: cancellationToken))
            return OperationResult.Failed(nameof(category.Name), $"Category with name {category.Name} already exists");
        context.Categories.Add(category);
        return await context.SaveChangesAsync(cancellationToken) > 0 ? OperationResult.Success("Category added successfully") : OperationResult.Failed(nameof(category.Id), "Category not added");
    }

    public async Task<OperationResult> UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        if (category.Id == 0)
            return OperationResult.Failed(nameof(category.Id), "Category Id is not valid");
        if (await IsCategoryNameTaken(category.Name, category.Id, cancellationToken))
            return OperationResult.Failed(nameof(category.Name), $"Category with name {category.Name} already exists");
        context.Categories.Update(category);
        return await context.SaveChangesAsync(cancellationToken) > 0 ? OperationResult.Success("Category updated successfully") : OperationResult.Failed(nameof(category.Id), "Category not updated");
    }

    public async Task<OperationResult> DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Categories.Where(c => c.Id == category.Id)
            .ExecuteDeleteAsync(cancellationToken) > 0
            ? OperationResult.Success("Category deleted successfully")
            : OperationResult.Failed(nameof(category.Id), "Category not deleted");
    }

    public async Task<bool> PaymentsExistAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Payments.AnyAsync(p => p.CategoryId == id, cancellationToken);
    }

    private async Task<bool> IsCategoryNameTaken(string name, int? id = null, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var lowerCaseName = name.ToLower();
        var taken = await context.Categories.AnyAsync(c => c.Name.ToLower() == lowerCaseName && c.Id != id, cancellationToken);
        return taken;
    }

    public async Task<List<CategoryByText>> GetCategoriesByTextAsync(int customerId, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var customer = await context.Customers.Include(c => c.Payments)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken: cancellationToken);

        var summaries = customer?.Payments.GroupBy(p => new { MonthYear = new DateTime(p.Date!.Value.Year, p.Date.Value.Month, 1), Category = p.Category.Name })
            .Select(g => new CategoryByText
            {
                MonthYear = g.Key.MonthYear,
                Category = g.Key.Category,
                TextCount = g.Count(),
                TotalCharacters = g.Sum(p => p.Amount),
                Total = g.Sum(p => p.Amount * p.Per1000 / 1000M)
            })
            .ToList() ?? new List<CategoryByText>();

        return summaries;
    }

    public async Task<List<CategorySummary>> GetCategorySummaryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var category = await context.Categories.Include(c => c.Payments)
            .ThenInclude(p => p.Customer)
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken: cancellationToken);

        var summaries = category?.Payments.GroupBy(p => new { MonthYear = new DateTime(p.Date!.Value.Year, p.Date.Value.Month, 1), Customer = p.Customer.Name })
            .Select(g => new CategorySummary
            {
                MonthYear = g.Key.MonthYear,
                Customer = g.Key.Customer,
                TextCount = g.Count(),
                Total = g.Sum(p => p.Amount * p.Per1000 / 1000M)
            })
            .ToList() ?? new List<CategorySummary>();

        return summaries;
    }
}