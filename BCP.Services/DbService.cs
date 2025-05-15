using BCP.Data.Entities;
using BCP.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BCP.Services
{
    public class DbService : IDbService
    {
        private readonly ILogger<DbService> _logger;
        public readonly IConfiguration _configuration;
        public readonly string _serviceUrl = string.Empty;

        public DbService(ILogger<DbService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceUrl = _configuration["DbService_URL"];
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            try
            {
                var tran = await HTTPClientWrapper<Transaction>.PostRequest(_serviceUrl, transaction);
                return tran;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbService CreateTransaction error");
                throw;
            }

        }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            try
            {
                var trans = await HTTPClientWrapper<IEnumerable<Transaction>>.Get(_serviceUrl);

                return trans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbService GetTransactions error");
                throw;
            }
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDate(DateTime date)
        {
            try
            {
                var trans = await HTTPClientWrapper<IEnumerable<Transaction>>.Get(_serviceUrl+ $"?date={date.ToString("yyyy-MM-dd")}");

                return trans;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbService GetTransactions error");
                throw;
            }
        }

        public async Task UpdateStatus(Transaction transaction)
        {
            try
            {
                await HTTPClientWrapper<Transaction>.PutRequest(_serviceUrl, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbService UpdateStatus error");
                throw;
            }
        }
    }
}
