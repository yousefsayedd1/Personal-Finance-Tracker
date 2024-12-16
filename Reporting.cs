namespace PersonalFinanceTracker
{
    public static class Reporting
    {
        public static string GeneralReport(Account account)
        {

            return $"your Total Income is {account.TotalIncomes}\nyour Total Expenses is {account.TotalExpenses}\nyour net worth is {account.Balance}";
        }
        public static string ActivityDateBasedReport(Account account, string category, DateTime startDate, DateTime endDate)
        {
            
            decimal totalIncomes = 0;
            decimal totalExpenses = 0;
            foreach (var income in account.incomes)
            {
                if (income.Date >=  startDate && income.Date <= endDate )
                {
                    totalIncomes += income.Amount;
                }    
            }
            foreach (var expense in account.expenses)
            {
                if (expense.Date >= startDate && expense.Date <= endDate)
                {
                    totalExpenses += expense.Amount;
                }
            }
            return $"between {startDate} and {endDate}:\nTotal Incomes: {totalIncomes}\nTotal Expenses: {totalExpenses}";
        }
    }
}
