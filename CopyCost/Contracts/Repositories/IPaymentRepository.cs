﻿using CopyCost.Entities;
using CopyCost.Extensions;

namespace CopyCost.Contracts.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetAllByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetAllByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<OperationResult> AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<OperationResult> UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<OperationResult> DeleteAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<bool> AnyCategoryCustomerExist(CancellationToken cancellationToken = default);
}