using System.ComponentModel.DataAnnotations;
using Customers.Api.Domain;
using Customers.Api.Mapping;
using Customers.Api.Repository;
using FluentValidation.Results;

namespace Customers.Api.Service;

public class CustomerService : ICustomerService
{

    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)

    {
        _customerRepository = customerRepository;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var existingUser = await _customerRepository.GetAsync(Guid.Parse("605d0a09-71ba-419e-ad13-ab3f8642de2a"));
        if (existingUser is not null)
        {
            var message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message);
        }

        var customerDto = customer.toCustomerDto();
        var result = await _customerRepository.CreateAsync(customerDto);
        return result;
        
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        var customerDto = await _customerRepository.GetAsync(id);
        
        return customerDto?.ToCustomer();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customerDtos = await _customerRepository.GetAllAsync();
        return customerDtos.Select(x => x.ToCustomer());
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        var customerDto = customer.toCustomerDto();
        return await _customerRepository.UpdateAsync(customerDto);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _customerRepository.DeleteAsync(id);
    }
}