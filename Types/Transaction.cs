﻿using System;

namespace Clidget.Core.Types
{
    public class Transaction
    {
        public TransactionType Type { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public Transaction(TransactionType type, int amount, DateTime date)
        {
            this.Type = type;
            this.Amount = amount;
            this.Date = date;
        }
    }
}