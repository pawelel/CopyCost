using CopyCost.CCExtensions;
using CopyCost.Contracts.Repositories;
using CopyCost.Data;
using CopyCost.Dto;
using CopyCost.Entities;
using Microsoft.EntityFrameworkCore;

namespace CopyCost.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IDbContextFactory<CopyCostDbContext> _contextFactory;

    public PaymentRepository(IDbContextFactory<CopyCostDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Payment?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Payments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var result = await context.Payments.Include(p=>p.Customer).Include(p=>p.Category).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<IEnumerable<Payment>> GetAllByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Payments.Where(p => p.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetAllByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Payments.Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<OperationResult> AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        if (payment.CustomerId == 0 || payment.CategoryId == 0)
            return OperationResult.Failed(nameof(payment.CustomerId), "Customer Id or Category Id is not valid");
        var customer = await context.Customers.FindAsync(new object[] { payment.CustomerId }, cancellationToken);
        if (customer == null)
            return OperationResult.Failed(nameof(payment.CustomerId), "Customer not found");
        var category = await context.Categories.FindAsync(new object[] { payment.CategoryId }, cancellationToken);
        if (category == null)
            return OperationResult.Failed(nameof(payment.CategoryId), "Category not found");
        payment.Customer = customer;
        payment.Category = category;
        await context.Payments.AddAsync(payment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return OperationResult.Success("Payment added successfully");
    }


    public async Task<OperationResult> UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var paymentExists = await PaymentExists(payment.Id, cancellationToken);
        if (!paymentExists)
            return OperationResult.Failed(nameof(Payment), "Payment not found");
        context.Payments.Update(payment);
        return await context.SaveChangesAsync(cancellationToken) > 0 ? OperationResult.Success("Payment updated successfully") : OperationResult.Failed(nameof(Payment), "Payment not updated");
    }

    public async Task<OperationResult> DeleteAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        context.Payments.Remove(payment);
        return await context.SaveChangesAsync(cancellationToken) > 0 ? OperationResult.Success("Payment deleted successfully") : OperationResult.Failed(nameof(Payment), "Payment not deleted");
    }

    public async Task<bool> AnyCategoryCustomerExist(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        // customer and category must exist
        return await context.Customers.AnyAsync(cancellationToken) && await context.Categories.AnyAsync(cancellationToken);
    }

    private async Task<bool> PaymentExists(int id, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Payments.AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<CustomerEarnings>> GetCustomerEarningsPerMonthAsync(int year, CancellationToken cancellationToken = default)
    {
        var payments = await GetAllAsync(cancellationToken);

        var customerEarningsList = new List<CustomerEarnings>();
        var groupedPayments = payments
            .Where(p => p.Date?.Year == year)
            .GroupBy(p => p.Customer.Name);

        foreach (var group in groupedPayments)
        {
            for (var month = 1; month <= 12; month++)
            {
                var monthlyEarnings = group.Where(p => p.Date?.Month == month).Sum(p => p.Total);
                customerEarningsList.Add(new CustomerEarnings { CustomerName = group.Key, Year = year, Month = month, Earnings = monthlyEarnings });
            }
        }

        return customerEarningsList;
    }


}