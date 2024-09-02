using System.ComponentModel.DataAnnotations;

namespace Customers.Contracts;

public class UpdateCustomerRequestBody
{
    [MaxLength(64)]
    public required string Name { get; init; }

    [MaxLength(16)]
    public required string Phone { get; init; }
}