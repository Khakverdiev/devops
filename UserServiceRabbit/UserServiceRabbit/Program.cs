using RabbitMQ.Client;
using System.Text;
using UserServiceRabbit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RabbitMqConnectionService>();
builder.Services.AddHostedService<RabbitMqConnectionService>();

builder.Services.AddSingleton<IConnection>(sp =>
{
    var rabbitService = sp.GetRequiredService<RabbitMqConnectionService>();
    
    if (rabbitService.Connection == null)
    {
        throw new InvalidOperationException("RabbitMQ connection is not established.");
    }
    
    return rabbitService.Connection!;
});

var app = builder.Build();

await app.Services.GetRequiredService<RabbitMqConnectionService>().StartAsync(CancellationToken.None);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/send", (IConnection connection, string message) =>
{
    using var channel = connection.CreateModel();

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

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
