using Customers.Api.Contracts.Responses;
using FastEndpoints;
using IocIdentity.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Customers.Api.Endpoints;
[HttpGet("auth/login"),AllowAnonymous]
public class LoginEndpoint : EndpointWithoutRequest<TokenResponse>
{
    
    private readonly IJwtBuilder _jwtBuilder;
    
    public LoginEndpoint(IJwtBuilder jwtBuilder)
    {
        _jwtBuilder = jwtBuilder;
    }
    
    
    public override async Task HandleAsync(CancellationToken ct)
    {

        var token = _jwtBuilder.GenerateToken();
        //create a json with generated token and return it
        var response = new TokenResponse
        {
            Token = token
        };
        
        await SendOkAsync(response, ct);

    }
    
    
}