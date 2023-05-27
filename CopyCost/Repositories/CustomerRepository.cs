using CopyCost.CCExtensions;
using CopyCost.Contracts.Repositories;
using CopyCost.Data;
using CopyCost.Entities;
using Microsoft.EntityFrameworkCore;

namespace CopyCost.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbContextFactory<CopyCostDbContext> _dbContextFactory;

    public CustomerRepository(IDbContextFactory<CopyCostDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Customer?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }


    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Customers.ToListAsync(cancellationToken);
    }

    public async Task<OperationResult> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {

        if (await IsCustomerNameTaken(customer.Name, cancellationToken: cancellationToken))
            return OperationResult.Failed(nameof(customer.Name), $"Customer with name {customer.Name} already exists");
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await context.Customers.AddAsync(customer, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken) > 0 ? OperationResult.Success("Customer added successfully") : OperationResult.Failed(nameof(customer.Id), "Customer not added");
    }

    public async Task<OperationResult> UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        if (customer.Id == 0)
            return OperationResult.Failed(nameof(customer.Id), "Customer Id is not valid");
        if (await IsCustomerNameTaken(customer.Name, customer.Id, cancellationToken))
            return OperationResult.Failed(nameof(customer.Name), $"Customer with name {customer.Name} already exists");
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        context.Customers.Update(customer);
        return await context.SaveChangesAsync(cancellationToken) > 0 ? OperationResult.Success("Customer updated successfully") : OperationResult.Failed(nameof(customer.Id), "Customer not updated");
    }

    public async Task<OperationResult> DeleteAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Customers.Where(c => c.Id == customer.Id)
            .ExecuteDeleteAsync(cancellationToken) > 0
            ? OperationResult.Success("Customer deleted successfully")
            : OperationResult.Failed(nameof(customer.Id), "Customer not deleted");
    }

    public async Task<bool> PaymentsExistAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Payments.AnyAsync(p => p.CustomerId == id, cancellationToken);
    }

    private async Task<bool> IsCustomerNameTaken(string name, int? id = null, CancellationToken cancellationToken = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var lowerCaseName = name.ToLower();
        return await context.Customers.AnyAsync(c => c.Name.ToLower() == lowerCaseName && c.Id != id, cancellationToken);

    }
}