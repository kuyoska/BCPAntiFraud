using BCP.Services;
using BCP.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using BCP.Api.TransactionService.Kafka;
using System.Text.Json;

namespace BCP.Api.TransactionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDbService _dbService;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly string _topicName = string.Empty;

        public TransactionController(IDbService dbService, ILogger <TransactionController> logger,
            IKafkaProducer kafkaProducer, IConfiguration configuration)
        {
            _logger = logger;
            _dbService = dbService;
            _kafkaProducer = kafkaProducer;
            _configuration = configuration;
            _topicName = _configuration["KafkaTopicName"];
        }

        [HttpPost(Name = "CreateTransaction")]
        public async Task<Transaction> Post([FromBody] Transaction transaction)
        {
            try
            {
                var transactionCreated = await _dbService.CreateTransaction(transaction);

                var tranJson = JsonSerializer.Serialize(transactionCreated);
                await _kafkaProducer.ProduceMessage(_topicName, tranJson);

                return transactionCreated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting transaction error");
                throw;
            }
        }

        [HttpGet(Name = "GetData")]
        public IActionResult Get()
        {
            return Ok($"topicName {_configuration["KafkaTopicName"]} dbServer {_configuration["DbService_URL"]}");
        }
    }
}
