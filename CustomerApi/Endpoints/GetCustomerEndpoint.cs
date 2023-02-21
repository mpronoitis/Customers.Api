using System.ComponentModel.DataAnnotations;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Customers.Api.Mapping;
using Customers.Api.Repository;
using Customers.Api.Service;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Customers.Api.Endpoints;


[HttpGet("customers/{id:guid}"), AllowAnonymous]
public class GetCustomerEndpoint : Endpoint<GetCustomerRequest, CustomerResponse>
{
    private readonly ICustomerService _customerService;
    
    public GetCustomerEndpoint(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public override async Task HandleAsync(GetCustomerRequest req, CancellationToken ct)
    {
        //get customer
        var customer = await _customerService.GetAsync(req.Id);

        if (customer is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var customerResponse = customer.ToCustomerResponse();
        await SendOkAsync(customerResponse, ct);
    }

    
}
