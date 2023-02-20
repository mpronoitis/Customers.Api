﻿using Customers.Api.Data;
using Customers.Api.Domain;

namespace Customers.Api.Repository;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(CustomerDto customer);
    
    Task<CustomerDto?> GetAsync(Guid id);
    
    Task<IEnumerable<CustomerDto>> GetAllAsync();

    Task<bool> UpdateAsync(CustomerDto customer);

    Task<bool> DeleteAsync(Guid id);
}