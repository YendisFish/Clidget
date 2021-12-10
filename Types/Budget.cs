namespace Clidget.Core.Types
{
    public class Budget
    {
        public string Title { get; set; }
        public long Amount { get; set; }

        public Budget(string title, long amount)
        {
            this.Title = title;
            this.Amount = amount;
        }
    }
}