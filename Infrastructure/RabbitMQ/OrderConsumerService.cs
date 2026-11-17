
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using OrdersWorkerService.Application.DTOs;
using OrdersWorkerService.Application.Interfaces;

using OrdersWorkerService.Infrastructure.Interfaces;

namespace OrdersWorkerService.Infrastructure.RabbitMQ
{
    public class OrderConsumerService(
        IConfiguration configuration,
        ILogger<OrderConsumerService> logger,
        IOrderValidationService orderValidationService,
        IServiceScopeFactory serviceScopeFactory)
        : IOrderConsumerService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<OrderConsumerService> _logger = logger;
        private readonly IOrderValidationService _orderValidationService = orderValidationService;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"]!,
                Port = _configuration.GetValue<int>("RabbitMQ:Port"),
                UserName = _configuration["RabbitMQ:Username"]!,
                Password = _configuration["RabbitMQ:Password"]!,
                AutomaticRecoveryEnabled = true
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken: stoppingToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            var queueName = _configuration["RabbitMQ:Queue"]!;

            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var order = JsonSerializer.Deserialize<OrderMessageDto>(message, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (order == null)
                    {
                        _logger.LogWarning("Message does not contain a valid order.");

                        await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
                    }
                    else
                    {
                        var errors = _orderValidationService.ValidateOrder(order);

                        if (errors.Count > 0)
                        {
                            _logger.LogWarning("\n Order {OrderId} invalid:\n - {Errors}", order.OrderId, string.Join("\n - ", errors));
                        }
                        else
                        {
                            _logger.LogInformation("Order {OrderId} validated successfully.", order.OrderId);

                            await using var scope = _serviceScopeFactory.CreateAsyncScope();
                            var orderProcessService = scope.ServiceProvider.GetRequiredService<IOrderProcessService>();

                            await orderProcessService.ProcessAsync(order, stoppingToken);

                            await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
                        }
                    }
                }
                catch (JsonException)
                {
                    _logger.LogWarning("Could not deserialize the message.");

                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when consuming the message.");

                    int retries = 3;

                    for (int i = 0; i < retries; i++)
                    {
                        await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true);
                    }
                }
            };

            await channel.BasicConsumeAsync(queueName, autoAck: false, consumer, stoppingToken);

            _logger.LogInformation("Background Worker has started consuming queue messages: {Queue}", queueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}