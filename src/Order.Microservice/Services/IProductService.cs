using Order.Microservice.DTO;
using Prodigy.HTTP;

namespace Order.Microservice.Services;

public interface IProductService
{
    Task<ProductDto> GetProductAsync(string id);
    Task<ProductDto> GetProductWithErrorAsync(string id);
}

public class ProductService : IProductService
{
    private readonly IHttpClient _httpClient;
    private readonly ILogger<ProductService> _logger;
    private const string _serviceUri = "https://localhost:7032/products";

    public ProductService(IHttpClient httpClient, ILogger<ProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task<ProductDto> GetProductAsync(string id)
    {
        _logger.LogInformation("Getting product id: {ProductId}", id);
        return _httpClient.GetAsync<ProductDto>($"{_serviceUri}/{id}");
    }

    public Task<ProductDto> GetProductWithErrorAsync(string id)
    {
        throw new NotImplementedException();
    }
}