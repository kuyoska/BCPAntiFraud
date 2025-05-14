using BCP.Data.Entities;

namespace BCP.Services
{
    public interface IDbService
    {
        Task<IEnumerable<Transaction>> GetTransactions();
        Task<IEnumerable<Transaction>> GetTransactionsByDate(DateTime date);
        Task<Transaction> CreateTransaction(Transaction transaction);
        Task UpdateStatus(Transaction transaction);
    }
}
