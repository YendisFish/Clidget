namespace Clidget.Core.Types
{
    public class Budget
    {
        public string Title { get; set; }
        public int Amount { get; set; }

        public Budget(string title, int amount)
        {
            this.Title = title;
            this.Amount = amount;
        }
    }
}