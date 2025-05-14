using BCP.Data.Entities;

namespace BCP.Api.AntiFraud
{
    public interface IAntiFraud
    {
        Task<Transaction> ValidateTransaction(Transaction transaction);
    }
}
