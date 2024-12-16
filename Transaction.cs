namespace PersonalFinanceTracker
{
    public abstract class Transaction
    {
        public decimal Amount { get; protected set; }
        public string Category { get; protected set; }
        public DateTime Date { get; protected set; }
        public Transaction(decimal amount, string category, DateTime date)
        {
            Amount = amount;
            Category = category;
            Date = date;
        }
        public override string ToString()
        {
            return $"Ammount: {Amount}, Category: {Category}, Date: {Date}";
        }
    }


}
