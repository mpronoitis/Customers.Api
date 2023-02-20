﻿using Customers.Api.Contracts.Requests;
using FluentValidation;

namespace Customers.Api.Middleware;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.DateOfBirth).NotEmpty();
    }
    
}