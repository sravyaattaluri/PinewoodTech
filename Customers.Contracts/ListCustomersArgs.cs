namespace Customers.Contracts;

public class ListCustomersArgs
{
    public int Page { get; set; } = 1;

    public int? CustomerId { get; set; }
}