namespace Order.Microservice.DTO
{
    public class BasketDto
    {
        public string Id { get; set; }
        public List<ProductBasketItemDto> Items { get; set; }
    }

    public class ProductBasketItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
