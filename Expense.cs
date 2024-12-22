namespace PersonalFinanceTracker
{
    public class  Expense : Transaction 
    {
        public Expense(decimal amount, string category, DateTime date) : base(amount, category, date)
        {
            Id = Ulid.NewUlid().ToString();
        }
        public Expense(string id, decimal amount, string category, DateTime date) : base(amount, category, date)
        {
            Id = id;

        }
    }


}
