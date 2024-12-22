namespace PersonalFinanceTracker.Interfaces
{
    public interface IUserInterface
    {
        void DisplayMessage(string message);
        void DisplayMenu();
        void DisplayCategories(List<string> categories);
        public Income ReadIncomeFromUser();
        public Expense ReadExpenceFromUser();

        int ReadInt(int min = int.MinValue, int max = int.MaxValue);
        string ReadString();
        decimal ReadDecimal(decimal min = decimal.MinValue, decimal max = decimal.MaxValue);
        DateTime ReadDate();
        void DisplayTransactions(IEnumerable<Transaction> transactions);
    }

}
