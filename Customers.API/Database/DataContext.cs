using Customers.API.Customers;
using Microsoft.EntityFrameworkCore;

namespace Customers.API.Database;

public class DataContext: DbContext
{
    private readonly IConfiguration _configuration;
    
    public DbSet<Customer> Customers { get; init; }
    
    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
    }
}