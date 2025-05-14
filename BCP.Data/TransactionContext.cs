using BCP.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BCP.Data
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {
            //LoadTestData();
        }

        private void LoadTestData()
        {
            Transactions.Add(new Transaction { Id = Guid.NewGuid(), CreatedAt = DateTime.Now.AddDays(-1), Amount = 10, SourceAccountId = Guid.NewGuid(), TargetAccountId = Guid.NewGuid() });
            Transactions.Add(new Transaction { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, Amount = 100, SourceAccountId = Guid.NewGuid(), TargetAccountId = Guid.NewGuid() });
            this.SaveChanges();
        }
    }
}
