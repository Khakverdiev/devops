using System.Text;
using OrderServiceRabbit;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<OrderServiceRabbitMqListener>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/send", (OrderServiceRabbitMqListener listener, string message) =>
{
    using var channel = listener.Channel;

    channel.QueueDeclare(queue: "order_queue",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "",
        routingKey: "order_queue",
        basicProperties: null,
        body: body);

    return Results.Ok($"Message sent: {message}");
});

app.Run();