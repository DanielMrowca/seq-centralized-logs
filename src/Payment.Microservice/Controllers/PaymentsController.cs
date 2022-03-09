using Microsoft.AspNetCore.Mvc;

namespace Payment.Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ILogger<PaymentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString("N"),
                IsSuccess = Convert.ToBoolean(new Random().Next(0, 1)),
                FinishedAt = DateTimeOffset.Now
            };

            if (!payment.IsSuccess)
                return BadRequest();


            _logger.LogInformation("Payment finished: {Payment}", payment);
            return new JsonResult(payment);
        }
    }

    public class Payment
    {
        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
    }
}