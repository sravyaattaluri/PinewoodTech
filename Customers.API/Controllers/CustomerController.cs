using Customers.API.Customers.Services;
using Customers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
    {
        var response = await _customerService.List(new ListCustomersArgs() { Page = 1 }, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomer(int id, CancellationToken cancellationToken)
    {
        var response = await _customerService.List(new ListCustomersArgs() { CustomerId = id }, cancellationToken);
        if (response.Items.Count == 0)
        {
            return NotFound();
        }
        return Ok(response.Items.First());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var response = await _customerService.Delete(id);
        if (response is null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerRequestBody body)
    {
        var response = await _customerService.Update(id, body);
        switch (response.Status)
        {
            case UpdateCustomerStatus.NotFound:
                return NotFound();
            case UpdateCustomerStatus.PhoneAlreadyExists:
                return Conflict(new { Message = "Phone Number already exists." });
            case UpdateCustomerStatus.Updated:
                return Ok(response.CustomerViewModel);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequestBody body)
    {
        var response = await _customerService.Create(body);

        switch (response.Status)
        {
            case CreateCustomerStatus.EmailAlreadyExists:
                return Conflict(new { Message = "Email already exists." });
            case CreateCustomerStatus.PhoneAlreadyExists:
                return Conflict(new { Message = "Phone Number already exists." });
            case CreateCustomerStatus.Created:
                return Ok(response.CustomerViewModel);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}