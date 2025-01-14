using RabbitMQ.Client;

namespace UserServiceRabbit;

public class RabbitMqConnectionService : IHostedService
{
    private readonly ConnectionFactory _factory;
    public IConnection? Connection { get; private set; }

    public RabbitMqConnectionService()
    {
        _factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            Connection = _factory.CreateConnection();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to RabbitMQ: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Connection?.Dispose();
        return Task.CompletedTask;
    }
}