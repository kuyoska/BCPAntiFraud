
using BCP.Api.AntiFraud.Kafka;
using BCP.Data.Entities;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BCP.Api.AntiFraud.HostedService
{
    public class ValidateTransactionHandler : BackgroundService
    {
        private readonly string _topicName = string.Empty;
        private readonly string _topicUpdateName = string.Empty;
        private readonly ILogger<ValidateTransactionHandler> _logger;
        private readonly IAntiFraud _antiFraud;
        private readonly IKafkaProducer _kafkaProducer;

        public ValidateTransactionHandler(IConfiguration configuration, ILogger<ValidateTransactionHandler> logger,
            IAntiFraud antiFraud, IKafkaProducer kafkaProducer)
        {
            _topicName = configuration["KafkaTopicName"];
            _topicUpdateName = configuration["KafkaTopicUpdateName"];
            _logger = logger;
            _antiFraud = antiFraud;
            _kafkaProducer = kafkaProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsumerConfig _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = "bcpDemo",
                GroupId = "my-group",
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig)
                .SetErrorHandler((_, ex) => _logger.LogError(ex.Reason))
                .Build())
            {
                consumer.Subscribe(_topicName);

                try
                {
                    while (true)
                    {
                        var consumerResult = consumer.Consume();
                        
                        var transaction = JsonSerializer.Deserialize<Transaction>(consumerResult.Message.Value);
                        _logger.LogInformation($"message received {transaction.Id} status {transaction.Status}");

                        //TODO:validate transaction
                        var transactionUpdated = await _antiFraud.ValidateTransaction(transaction);

                        _logger.LogInformation($"Validate transaction {transactionUpdated.Id} status {transactionUpdated.Status}");

                        var tranJson = JsonSerializer.Serialize(transactionUpdated);
                        await _kafkaProducer.ProduceMessage(_topicUpdateName, tranJson);

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ExecuteAsync");
                    throw;
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}
