using Customers.Api.Contracts.Responses;
using Customers.Api.Database;
using Customers.Api.Middleware;
using Customers.Api.Repository;
using Customers.Api.Service;
using FastEndpoints;
using FastEndpoints.Swagger;
using IocIdentity;
using NJsonSchema.Validation;

var builder = WebApplication.CreateBuilder(args);
//get configuration
var config = builder.Configuration;

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();

//add Services to container
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
        new SqlLiteConnectionFactory(config.GetValue<string>("Database:ConnectionString")));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

builder.Services.AddApiConfig(config);

var app = builder.Build();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseFastEndpoints(x =>
{
    //Errors
    x.Errors.ResponseBuilder = (failures, _,builder) =>
    {
        return new ValidationFailureResponse
        {
            Errors = failures.Select(x => x.ErrorMessage).ToList()
        };
    };
    
});
app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.UseAuthentication();
app.UseAuthorization();
//take DatabaseInitializer service from container
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.Run();