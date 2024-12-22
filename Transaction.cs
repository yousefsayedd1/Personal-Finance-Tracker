namespace PersonalFinanceTracker
{
    public abstract class Transaction
    {
        public string Id { get; protected set; }
        public decimal Amount { get; protected set; }
        public string Category { get; protected set; }
        public DateTime Date { get; protected set; }
        public Transaction(decimal amount, string category, DateTime date)
        {
            Amount = amount;
            Category = category;
            Date = date;
        }
        public static bool operator ==(Transaction t1, Transaction t2)
        {
            return t1.Id == t2.Id;
        }public static bool operator !=(Transaction t1, Transaction t2)
        {
            return t1.Id != t2.Id;
        }
        public override string ToString()
        {
            return $"Amount: {Amount}, Category: {Category}, Date: {Date}";
        }
    }


}
