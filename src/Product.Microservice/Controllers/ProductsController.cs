using Microsoft.AspNetCore.Mvc;

namespace Product.Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly string[] Products = new[]
        {
            "TV LED", "MS Keyboard", "Yamaha Motif XF7", "Electric Drums", "CS Source", "GeForce GTX 7777"
        };

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            var product = new Product
            {
                Id = id,
                Name = Products.ElementAt(new Random().Next(0, Products.Length - 1))
            };

            _logger.LogInformation("Returning product id: {ProductId}. {@Product}", product.Id, product);
            return new JsonResult(product);
        }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}