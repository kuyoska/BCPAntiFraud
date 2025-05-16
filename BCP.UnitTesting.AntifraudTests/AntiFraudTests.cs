using BCP.Data.Entities;
using BCP.Api.AntiFraud;
using BCP.Services;
using Moq;
using System.Threading.Tasks;

namespace BCP.UnitTesting.AntifraudTests
{
    public class AntiFraudTests
    {
        private readonly Mock<IDbService> _bServiceMock = new Mock<IDbService>();

        [Fact]
        public async Task ShouldThrowExceptionIfTransactionIsNull()
        {
            //arrange
            Transaction transaction = null;
            var antifraud = new AntiFraud(_bServiceMock.Object);

            //act and assert
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await antifraud.ValidateTransaction(transaction));            
        }

        [Fact]
        public async Task ShouldReturnApprovedStatusWhenNoTransactions()
        {
            //arrange
            var transactionsNoData = new List<Transaction>();

            _bServiceMock.Setup(z => z.GetTransactionsByDate(It.IsAny<DateTime>()))
                .ReturnsAsync(transactionsNoData);

            Transaction transaction = new Transaction { Amount = 10 };
            var antifraud = new AntiFraud(_bServiceMock.Object);

            //act
            var result = await antifraud.ValidateTransaction(transaction);

            //arrange
            Assert.Equal(TransactionStatus.Approved, result.Status);
        }

        [Fact]
        public async Task ShouldReturnApprovedStatusWhenTransactionsLowerThan20000()
        {
            //arrange
            var transactionsData = new List<Transaction>();
            transactionsData.Add(new Transaction { Amount = 1000 });
            transactionsData.Add(new Transaction { Amount = 2330 });
            transactionsData.Add(new Transaction { Amount = 7000 });
            transactionsData.Add(new Transaction { Amount = 100 });

            _bServiceMock.Setup(z => z.GetTransactionsByDate(It.IsAny<DateTime>()))
                .ReturnsAsync(transactionsData);

            Transaction transaction = new Transaction { Amount = 10 };
            var antifraud = new AntiFraud(_bServiceMock.Object);

            //act
            var result = await antifraud.ValidateTransaction(transaction);

            //arrange
            Assert.Equal(TransactionStatus.Approved, result.Status);
        }

        [Fact]
        public async Task ShouldReturnRejectedStatusWhenTransactionsAmountGreaterThan2000()
        {
            //arrange
            var transactionsData = new List<Transaction>();
            transactionsData.Add(new Transaction { Amount = 1000 });
            transactionsData.Add(new Transaction { Amount = 2330 });
            transactionsData.Add(new Transaction { Amount = 7000 });
            transactionsData.Add(new Transaction { Amount = 100 });

            _bServiceMock.Setup(z => z.GetTransactionsByDate(It.IsAny<DateTime>()))
                .ReturnsAsync(transactionsData);

            Transaction transaction = new Transaction { Amount = 3000 };
            var antifraud = new AntiFraud(_bServiceMock.Object);

            //act
            var result = await antifraud.ValidateTransaction(transaction);

            //arrange
            Assert.Equal(TransactionStatus.Rejected, result.Status);
        }

        [Fact]
        public async Task ShouldReturnRejectedStatusWhenTransactionsAmountGreaterThan20000()
        {
            //arrange
            var transactionsData = new List<Transaction>();
            transactionsData.Add(new Transaction { Amount = 1000 });
            transactionsData.Add(new Transaction { Amount = 2330 });
            transactionsData.Add(new Transaction { Amount = 7000 });
            transactionsData.Add(new Transaction { Amount = 15000 });

            _bServiceMock.Setup(z => z.GetTransactionsByDate(It.IsAny<DateTime>()))
                .ReturnsAsync(transactionsData);

            Transaction transaction = new Transaction { Amount = 20 };
            var antifraud = new AntiFraud(_bServiceMock.Object);

            //act
            var result = await antifraud.ValidateTransaction(transaction);

            //arrange
            Assert.Equal(TransactionStatus.Rejected, result.Status);
        }
    }
}