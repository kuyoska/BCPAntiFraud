using BCP.Api.TransactionService.Controllers;
using BCP.Api.TransactionService.Kafka;
using BCP.Data.Entities;
using BCP.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BCP.UnitTesting.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<IDbService> _bServiceMock = new Mock<IDbService>();
        private readonly Mock<ILogger<TransactionController>> _loggerMock = new Mock<ILogger<TransactionController>>();
        private readonly Mock<IKafkaProducer> _kafkaProducerMock = new Mock<IKafkaProducer>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();

        [Fact]
        public async Task ShouldCreateNewGUIDForTransaction()
        {
            //arrange
            var newGuid = Guid.NewGuid();
            _bServiceMock.Setup(b => b.CreateTransaction(It.IsAny<Transaction>()))
                .ReturnsAsync(new Transaction { Id = newGuid });

            TransactionController controller = new TransactionController(_bServiceMock.Object,
                _loggerMock.Object, _kafkaProducerMock.Object, _configurationMock.Object);

            var transaction = new Transaction { Amount = 20 };

            //act 
            var result = await controller.Post(transaction);

            //assert
            Assert.Equal(newGuid, result.Id);
        }

    }
}
