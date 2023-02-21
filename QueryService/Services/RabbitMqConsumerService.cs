using CommandService.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QueryService.DataAccess;
using QueryService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueryService.Services;

public class RabbitMqConsumerService : BackgroundService, IDisposable
    {
        private readonly ILogger<RabbitMqConsumerService> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMqConfig _rabbitMqConfig;

        private readonly IBookUpdateService _bookUpdateService;

        public RabbitMqConsumerService(ILoggerFactory loggerFactory, IOptions<RabbitMqConfig> options, IBookUpdateService bookUpdateService)
        {
            _bookUpdateService = bookUpdateService;
            _logger = loggerFactory.CreateLogger<RabbitMqConsumerService>();
            _rabbitMqConfig = options.Value;
            InitializeMessageQueue();
        }

        private void InitializeMessageQueue()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.UserName,
                Password = _rabbitMqConfig.Password,
                VirtualHost = _rabbitMqConfig.VirtualHost,
                Port = _rabbitMqConfig.Port
            };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("ms-exchange", ExchangeType.Topic);
            _channel.QueueDeclare("ms-queue", false, false, false, null);
            _channel.QueueBind("ms-queue", "ms-exchange", "cqrs", null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += (sender, args) =>
            {
                _logger.LogInformation($"Message queue connection shutting down...");
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, args) =>
            {
                var messageString = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonConvert.DeserializeObject<BookMessageViewModel>(messageString);

                _logger.LogInformation($"Message received: {Environment.NewLine}{message.BookId}{Environment.NewLine}{message.CommandType}");

                await _bookUpdateService.UpdateAsync(message);
                
                _channel.BasicAck(args.DeliveryTag, false);
            };

            //  Also consider other events on consumer

            _channel.BasicConsume("ms-queue", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }