using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Customers.Api.Service;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Customers.Api.Endpoints;

[HttpDelete("customers/{id:guid}"),AllowAnonymous]
public class DeleteCustomerEndpoint : Endpoint<DeleteCustomerRequest,CustomerResponse>
{
    private readonly ICustomerService _customerService;
    
    public DeleteCustomerEndpoint(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public override async Task HandleAsync(DeleteCustomerRequest req, CancellationToken ct)
    {
        var deletedCustomer = await _customerService.DeleteAsync(req.Id);
        
        if (!deletedCustomer)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);

    }
}