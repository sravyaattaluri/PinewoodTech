namespace Customers.API;

public record ListResponse<T>(ICollection<T> Items);