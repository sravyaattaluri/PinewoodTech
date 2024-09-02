
using Customers.Contracts;

namespace Customers.API.Customers.Services;

public interface ICustomerService
{
    Task<(CreateCustomerStatus Status, CustomerViewModel? CustomerViewModel)> Create(CreateCustomerRequestBody body);
    Task<CustomerViewModel?> Delete(int id);
    Task<(UpdateCustomerStatus Status, CustomerViewModel? CustomerViewModel)> Update(int customerId, UpdateCustomerRequestBody body);
    Task<ListResponse<CustomerViewModel>> List(ListCustomersArgs args, CancellationToken cancellationToken);
}