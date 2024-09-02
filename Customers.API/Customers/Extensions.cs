using Customers.Contracts;

namespace Customers.API.Customers;

public static class Extensions
{
    public static CustomerViewModel ToViewModel(this Customer customer)
    {
        return new CustomerViewModel(customer.CustomerId, customer.Name, customer.Email, customer.Phone);
    }
}