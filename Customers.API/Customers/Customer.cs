using System.ComponentModel.DataAnnotations;

namespace Customers.API.Customers;

public class Customer
{
    public int CustomerId { get; init; }

    [MaxLength(64)]
    public required string Name { get; set; }

    [MaxLength(64)]
    public required string Email { get; set; }

    [MaxLength(16)]
    public required string Phone { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }
}