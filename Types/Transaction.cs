using System;

namespace Clidget.Core.Types
{
    public class Transaction
    {
        public TransactionType Type { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
        public string? RunningBalance { get; set; }

        public string? Message { get; set; }

        public Budget? Budget { get; set; }

        public Transaction(TransactionType type, int amount, DateTime date, string? message, string runningBalance = null, Budget budget = null)
        {
            this.Type = type;
            this.Amount = amount;
            this.Date = date;
            this.Message = message;
            this.RunningBalance = runningBalance;
            this.Budget = budget;
        }
    }
}