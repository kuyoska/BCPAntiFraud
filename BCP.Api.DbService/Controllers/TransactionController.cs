using BCP.Data;
using BCP.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BCP.Api.DbService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly TransactionContext _transactionContext;

        public TransactionController(ILogger<TransactionController> logger,
            TransactionContext dbContext)
        {
            _logger = logger;
            _transactionContext = dbContext;
        }


        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<Transaction> Get(DateTime? date)
        {
            try
            {
                if (date == null)
                    return _transactionContext.Transactions.ToList();
                else
                    return _transactionContext.Transactions.Where(t => t.CreatedAt.Date == date.Value.Date).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetTransactions error");
                throw;
            }
        }

        [HttpPost(Name = "CreateTransaction")]
        public Transaction Post([FromBody] Transaction transaction)
        {
            try
            {
                _transactionContext.Transactions.Add(transaction);
                _transactionContext.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateTransaction error");
                throw;
            }
        }

        [HttpPut(Name = "UpdateStatus")]
        public Transaction Put([FromBody] Transaction transaction)
        {
            try
            {
                var currentTran = _transactionContext.Transactions.FirstOrDefault(t => t.Id == transaction.Id);

                if (currentTran != null)
                {
                    currentTran.Status = transaction.Status;
                    _transactionContext.SaveChanges();
                }

                return currentTran;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateStatus error");
                throw;
            }
        }
    }
}
