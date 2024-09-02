using System.ComponentModel.DataAnnotations;

namespace Customers.Contracts;

public class CreateCustomerRequestBody
{
    [MaxLength(64)]
    public required string Name { get; init; }

    [MaxLength(64)]
    public required string Email { get; init; }

    [MaxLength(16)]
    public required string Phone { get; init; }
}