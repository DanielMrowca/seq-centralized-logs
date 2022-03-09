namespace Order.Microservice.DTO
{
    public class PaymentDto
    {
        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
    }
}
