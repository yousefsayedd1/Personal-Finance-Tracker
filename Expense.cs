namespace PersonalFinanceTracker
{
    public class  Expense : Transaction 
    {
        public Expense(decimal amount, string category, DateTime date) : base(amount, category, date)
        {

        }
    }


}
