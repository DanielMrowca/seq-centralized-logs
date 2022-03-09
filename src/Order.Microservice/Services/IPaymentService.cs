using Order.Microservice.DTO;
using Prodigy.HTTP;

namespace Order.Microservice.Services;

public interface IPaymentService
{
    Task<PaymentDto> MakePaymentSuccessAsync(string orderId, double totalPrice);
    Task<PaymentDto> MakePaymentWithErrorAsync();
}

public class PaymentService : IPaymentService
{
    private readonly IHttpClient _httpClient;
    private readonly ILogger<PaymentService> _logger;
    private const string _serviceUri = "https://localhost:7127/payments";

    public PaymentService(IHttpClient httpClient, ILogger<PaymentService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task<PaymentDto> MakePaymentSuccessAsync(string orderId, double totalPrice)
    {
        _logger.LogInformation("Requesting payment for orderId: {OrderId}, TotalPrice: {TotalPrice}", orderId, totalPrice );
        return _httpClient.GetAsync<PaymentDto>($"{_serviceUri}/{orderId}");
    }

    public Task<PaymentDto> MakePaymentWithErrorAsync()
    {
        throw new NotImplementedException();
    }
}