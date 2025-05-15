using BCP.Data.Entities;
using BCP.Services;

namespace BCP.Api.AntiFraud
{
    public class AntiFraud : IAntiFraud
    {
        private readonly IDbService _dbService;

        public AntiFraud(IDbService dbService) 
        {
            _dbService = dbService;
        }

        public async Task<Transaction> ValidateTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                var transactionStatus = TransactionStatus.Approved;
                if (transaction.Amount > 2000)
                    transactionStatus = TransactionStatus.Rejected;

                var getTodayTransactions = await _dbService.GetTransactionsByDate(DateTime.Today);

                var totalAmountForToday = getTodayTransactions.Sum(x => x.Amount);

                if(totalAmountForToday > 20000)
                    transactionStatus = TransactionStatus.Rejected;

                transaction.Status = transactionStatus;
                return transaction;
            }
            else
                throw new ArgumentException("Transaction is empty");
        }
    }
}
