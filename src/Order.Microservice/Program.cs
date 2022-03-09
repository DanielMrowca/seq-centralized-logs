using Order.Microservice.Services;
using Shared;

var logger = Prodigy.Logging.Extensions.BuildLoggerConfiguration().CreateLogger();
logger.Information("{MicroserviceName} is starting...", "Order.Microservice");


var builder = WebApplication.CreateBuilder(args);
builder.AddCore();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
