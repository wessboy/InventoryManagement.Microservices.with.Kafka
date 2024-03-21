using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InventoryConsumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore,string> _consumer;
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(IConfiguration configuration,ILogger<ConsumerService> logger)
        {
            _logger = logger;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["kafka:BootstrapServers"],
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();  
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("InventoryUpdates");

            while(!stoppingToken.IsCancellationRequested)
            { 
               
             ProcessKafkaMessage(stoppingToken);
            
            }
        }


        public void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;

                _logger.LogInformation($"Recived inventory update: {message}");
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error processing kafka message: {ex.Message}");
            }
        }
    }
}
