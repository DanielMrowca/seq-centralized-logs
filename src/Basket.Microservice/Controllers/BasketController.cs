using Microsoft.AspNetCore.Mvc;

namespace Basket.Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly Basket _basket;

        public BasketController(ILogger<BasketController> logger)
        {
            _logger = logger;
            _basket = Basket.Create();
        }

        //[HttpGet]
        //[Route("{id}")]
        //public IActionResult GetProduct([FromRoute] string id)
        //{

        //}

        [HttpPost(Name = "AddProductToBasket")]
        public IActionResult AddProductToBasket()
        {
            _logger.LogInformation($"Product with id: '{_basket.ProductId}' added to basket id: '{_basket.Id}'");
            return Ok();
        }

        [HttpPost(Name = "AddProductToBasketWithMessageTemplate")]
        public IActionResult AddProductToBasketTemplate()
        {
            _logger.LogInformation("Product with id: {ProductId} added to basket id: {BasketId}", _basket.ProductId, _basket.Id);
            return Ok();
        }

        [HttpPost(Name = "AddProductToBasketWithError")]
        public IActionResult AddProductToBasketWithError()
        {
            _logger.LogInformation("Unable to add product id: {ProductId} to basket id: {BasketId}", _basket.ProductId, _basket.Id);
            return BadRequest(400);
        }
    }

    public class Basket
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        public static Basket Create() => new Basket
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = Guid.NewGuid().ToString(),
            Quantity = new Random().Next(1, 5)
        };
    }
}