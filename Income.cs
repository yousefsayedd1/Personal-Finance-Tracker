namespace PersonalFinanceTracker
{
    public class Income : Transaction {

        public Income(decimal amount, string category, DateTime date) : base(amount, category, date)
        {
            Id  = Ulid.NewUlid().ToString();
        }
        public Income(string id, decimal amount, string category, DateTime date) : base(amount, category, date)
        {
            Id = id;
           
        }

    }


}
