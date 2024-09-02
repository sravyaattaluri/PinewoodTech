using System.Diagnostics;
using Customers.API;
using Customers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Customers.UI.Models;

namespace Customers.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        HttpClient client = new();
        client.BaseAddress = new Uri("http://localhost:5284");
        var customers = await client.GetFromJsonAsync<ListResponse<CustomerViewModel>>("/api/Customers");
        return View(customers);
    }

    public IActionResult CreateCustomer()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CustomerViewModel customerViewModel)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri("http://localhost:5284");
        var response = await client.PostAsJsonAsync("/api/Customers",
            new CreateCustomerRequestBody
                { Email = customerViewModel.Email, Name = customerViewModel.Name, Phone = customerViewModel.Phone });

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return View();
    }

    [HttpGet("/EditCustomer/{customerId:int}")]
    public async Task<IActionResult> EditCustomer(int customerId)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri("http://localhost:5284");
        var response = await client.GetFromJsonAsync<CustomerViewModel>($"/api/Customers/{customerId}");

        if (response is null)
        {
            return RedirectToAction("Index");
        }

        return View(response);
    }

    [HttpPost("/EditCustomer/{customerId:int}")]
    public async Task<IActionResult> EditCustomer(int customerId, CustomerViewModel customerViewModel)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri("http://localhost:5284");
        var response = await client.PutAsJsonAsync($"/api/Customers/{customerId}",
            new UpdateCustomerRequestBody
                { Name = customerViewModel.Name, Phone = customerViewModel.Phone });

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return View();
    }

    public async Task<IActionResult> DeleteCustomer(int customerId)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri("http://localhost:5284");
        await client.DeleteAsync($"/api/Customers/{customerId}");
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}