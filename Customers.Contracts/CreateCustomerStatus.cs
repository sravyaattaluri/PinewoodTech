namespace Customers.Contracts;

public enum CreateCustomerStatus
{
    Created,
    EmailAlreadyExists,
    PhoneAlreadyExists,
}