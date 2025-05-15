
using System.Text.Json;
using BCP.Data.Entities;
using BCP.Services;
using Confluent.Kafka;

namespace BCP.Api.TransactionService.HostedService
{
    public class UpdateTransactionHandler : BackgroundService
    {
        private readonly string _topicUpdateName = string.Empty;
        private readonly ILogger<UpdateTransactionHandler> _logger;
        private readonly IDbService _dbService;

        public UpdateTransactionHandler(IConfiguration configuration, ILogger<UpdateTransactionHandler> logger,
            IDbService dbService)
        {
            _topicUpdateName = configuration["KafkaTopicUpdateName"];
            _logger = logger;
            _dbService = dbService;
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
                consumer.Subscribe(_topicUpdateName);

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumerResult = consumer.Consume();
                        var transaction = JsonSerializer.Deserialize<Transaction>(consumerResult.Message.Value);

                        _logger.LogInformation($"message received {transaction.Id} status {transaction.Status}");

                        //TODO:update transaction
                        await _dbService.UpdateStatus(transaction);
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
