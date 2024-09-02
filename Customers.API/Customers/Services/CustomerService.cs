using Customers.API.Customers.Options;
using Customers.API.Database;
using Customers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Customers.API.Customers.Services;

public class CustomerService: ICustomerService
{
    private readonly DataContext _context;
    private readonly IOptions<CustomerOptions> _settings;

    public CustomerService(DataContext context, IOptions<CustomerOptions> settings)
    {
        _context = context;
        _settings = settings;
    }

    
    
    public async Task<(CreateCustomerStatus Status, CustomerViewModel? CustomerViewModel)> Create(CreateCustomerRequestBody body)
    {
        var customerWithEmail = await _context.Customers.FirstOrDefaultAsync(x => x.Email.ToLower() == body.Email.ToLower());
        if (customerWithEmail is not null)
        {
            return (CreateCustomerStatus.EmailAlreadyExists, null);
        }
        
        var customerWithPhone = await _context.Customers.FirstOrDefaultAsync(x => x.Phone.ToLower() == body.Phone.ToLower());
        if (customerWithPhone is not null)
        {
            return (CreateCustomerStatus.PhoneAlreadyExists, null);
        }

        var customer = new Customer()
            { Email = body.Email, Phone = body.Phone, Name = body.Name, Created = DateTime.UtcNow };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return (CreateCustomerStatus.Created, customer.ToViewModel());
    }

    public async Task<CustomerViewModel?> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return null;
        }
        
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return customer.ToViewModel();
    }


    public async Task<(UpdateCustomerStatus Status, CustomerViewModel? CustomerViewModel)> Update(int customerId, UpdateCustomerRequestBody body)
    {
        var customerWithPhone = await _context.Customers.FirstOrDefaultAsync(x => x.Phone == body.Phone && x.CustomerId != customerId);
        if (customerWithPhone is not null)
        {
            return (UpdateCustomerStatus.PhoneAlreadyExists, null);
        }
        
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
        {
            return (UpdateCustomerStatus.NotFound, null);
        }
        
        customer.Name = body.Name;
        customer.Phone = body.Phone;
        customer.Updated = DateTime.UtcNow;
        
        _context.Update(customer);
        await _context.SaveChangesAsync();
        
        return (UpdateCustomerStatus.Updated, customer.ToViewModel());
    }

    public async Task<ListResponse<CustomerViewModel>> List(ListCustomersArgs args, CancellationToken cancellation)
    {
        List<CustomerViewModel> customers = new();

        if (args.CustomerId.HasValue)
        {
            customers = await _context.Customers.Where(x => x.CustomerId == args.CustomerId).Select(x => new CustomerViewModel(x.CustomerId, x.Name, x.Email, x.Phone)).ToListAsync(cancellation);
        }
        else
        {
            customers = await _context.Customers
            .Skip((args.Page - 1) * _settings.Value.PageSize)
            .Take(_settings.Value.PageSize)
            .Select(x => new CustomerViewModel(x.CustomerId, x.Name, x.Email, x.Phone))
            .ToListAsync(cancellation);
        }
        return new ListResponse<CustomerViewModel>(customers);


    }
}