
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using OrdersWorkerService.Validators;
using OrdersWorkerService.DTOs;
using System.Text.Json;

namespace OrdersWorkerService.Services
{
    public class OrderConsumerService(IConfiguration configuration, ILogger<OrderConsumerService> logger, OrderValidator validator)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<OrderConsumerService> _logger = logger;
        private readonly OrderValidator _validator = validator;

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

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false,
                cancellationToken: stoppingToken);

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
                        _logger.LogWarning("Message without a vadid order.");
                    }
                    else
                    {
                        var errors = _validator.Validate(order);

                        if (errors.Count > 0)
                        {
                            _logger.LogWarning("\n Order {OrderId} invalid:\n - {Errors}", order.OrderId, string.Join("\n - ", errors));
                        }
                        else
                        {
                            _logger.LogInformation("Order {OrderId} validated with success.", order.OrderId);
                        }
                    }




                    // Apenas para aprendizagem nesta fase.
                    // Na implementação final, só fazemos ACK
                    // depois de guardar a encomenda no PostgreSQL.
                    await channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false);
                }
                catch (JsonException)
                {
                    _logger.LogWarning("Not possible to deserialize message.");

                    await channel.BasicAckAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error when consuming the message.");

                    // Recoloca a mensagem na fila.
                    // Adicionar retries, para prevenir loop infinito
                    await channel.BasicNackAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false,
                        requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("Worker has started consuming queue messages: {Queue}", queueName);

            // Mantém a ligação aberta enquanto
            // o Worker estiver em execução.
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}