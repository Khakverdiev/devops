using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderServiceRabbit;

public class OrderServiceRabbitMqListener : IHostedService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "order_queue";

    public IModel Channel => _channel;

    public OrderServiceRabbitMqListener()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        Console.WriteLine("RabbitMQ connection established.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {message}");
        };

        _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();
        return Task.CompletedTask;
    }
}