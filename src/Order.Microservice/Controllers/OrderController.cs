using Microsoft.AspNetCore.Mvc;
using Order.Microservice.Services;

namespace Order.Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly IServiceProvider _serviceProvider;

        public OrderController(
            ILogger<OrderController> logger,
            IProductService productService,
            IServiceProvider serviceProvider,
            IPaymentService paymentService)
        {
            _logger = logger;
            _productService = productService;
            _serviceProvider = serviceProvider;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderWithSuccess()
        {
            var order = Order.Create();
            try
            {
                _logger.LogInformation("Created order with Id: {OrderId}. {@Order}", order.Id, order);
                var product = await _productService.GetProductAsync(order.OrderDetails.FirstOrDefault()?.ProductId);

                _logger.LogInformation("Making payment for order id: {OrderId}", order.Id);
                var payment = await _paymentService.MakePaymentSuccessAsync(order.Id, 99.99);

                _logger.LogInformation("Order id: {OrderId} payment finished with success. ", order.Id);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Order id: {OrderId} failed. ", order.Id);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("unhandledException")]
        public Task<IActionResult> CreateOrderWithError()
        {
            _serviceProvider.GetRequiredService<INotRegisteredService>();
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPost]
        [Route("warning")]
        public Task<IActionResult> CreateOrderWithWarning()
        {
            var order = Order.Create();
            _logger.LogWarning("Order id: {OrderId} payment cannot be finished. Try again later.", order.Id);
            return Task.FromResult<IActionResult>(Ok());
        }


        [HttpPost]
        [Route("critical")]
        public Task<IActionResult> CreateOrderWithFatal()
        {
            var order = Order.Create();
            _logger.LogCritical("Order id: {OrderId} took your money but finished with error.", order.Id);
            return Task.FromResult<IActionResult>(Ok());
        }


        [HttpPost]
        [Route("trace")]
        public Task<IActionResult> CreateOrderWithTrace()
        {
            var order = Order.Create();
            _logger.LogTrace("Order id: {OrderId} Trace Log example.", order.Id);
            return Task.FromResult<IActionResult>(Ok());
        }

        [HttpPost]
        [Route("debug")]
        public Task<IActionResult> CreateOrderWithDebug()
        {
            var order = Order.Create();
            _logger.LogDebug("Order id: {OrderId} Debug Log example.", order.Id);
            return Task.FromResult<IActionResult>(Ok());
        }
    }

    public class Order
    {
        public string Id { get; set; }
        public string PaymentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public bool HasBeenShipped { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public static Order Create()
        {
            var orderId = Guid.NewGuid().ToString("N");
            return new Order
            {
                Id = orderId,
                PaymentId = Guid.NewGuid().ToString("N"),
                FirstName = "Daniel",
                LastName = "Mrowca",
                Address = "Krupnicza",
                City = "Cracow",
                HasBeenShipped = false,
                PostalCode = "30-123",
                Date = DateTimeOffset.Now,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ProductId = Guid.NewGuid().ToString("N"),
                        OrderId = orderId,
                        Quantity = 1
                    },
                    new OrderDetail
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ProductId = Guid.NewGuid().ToString("N"),
                        OrderId = orderId,
                        Quantity = 1
                    }
                }
            };
        }
    }

    public class OrderDetail
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}