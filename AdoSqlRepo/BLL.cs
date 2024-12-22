using System.Data;


namespace PersonalFinanceTracker.AdoSqlRepo

{
    public static class BLL
    {
        public static List<Income> GetAllAccountIncomes(string accountId)
        {

            DataTable dataTable = DAL.GetAllAccountIncomes(accountId);
            List<Income> incomes = new List<Income>();
            foreach (DataRow row in dataTable.Rows)
            {
                string Id = row["Id"]?.ToString();
                decimal Amount = Convert.ToDecimal(row["Amount"]);
                string category = row["Category"]?.ToString();
                DateTime Date = row.IsNull("Date") ? DateTime.MinValue : Convert.ToDateTime(row["Date"]);

                incomes.Add(new Income(Id, Amount, category, Date));
            }
            return incomes;
        }
        public static List<Expense> GetAllAccountExpenses(string accountId)
        {
            DataTable dataTable = DAL.GetAllAccountExpenses(accountId);
            List<Expense> expenses = new List<Expense>();
            foreach (DataRow row in dataTable.Rows)
            {
                string Id = row["Id"]?.ToString();
                decimal Amount = Convert.ToDecimal(row["Amount"]);
                string category = row["Category"]?.ToString();
                DateTime Date = Convert.ToDateTime(row["Date"]);
                expenses.Add(new Expense(Id, Amount, category, Date));
            }
            return expenses;
        }
        public static List<string> GetAllCategories(string accountId)
        {
            List<string> categories = new List<string>();
            DataTable dataTable = DAL.GetAllCategoriesByAccountId(accountId);
            foreach (DataRow row in dataTable.Rows)
            {
                string name = row["Name"]?.ToString();
                categories.Add(name);
            }
            return categories;
        }
        public static Dictionary<string, KeyValuePair<decimal, decimal>> GetAllCategoriesBudget(string accountId)
        {
            Dictionary<string, KeyValuePair<decimal, decimal>> categoriesBudget = new Dictionary<string, KeyValuePair<decimal, decimal>>();
            DataTable dataTable = DAL.GetAllCategoriesBudgetByAccountId(accountId);
            foreach (DataRow row in dataTable.Rows)
            {
                string name = row["Name"]?.ToString();
                if (row.IsNull("Budget"))
                    continue;
                decimal budget = Convert.ToDecimal(row["Budget"]);
                categoriesBudget.Add(name, new KeyValuePair<decimal, decimal>(0, budget));
            }
            return categoriesBudget;
        }
        public static Account GetAccountById(string accountId)
        {
            try
            {
                List<Income> Incomes = GetAllAccountIncomes(accountId);
                List<Expense> Expenses = GetAllAccountExpenses(accountId);
                List<string> Categories = GetAllCategories(accountId);
                Dictionary<string, KeyValuePair<decimal, decimal>> CategoryBudget = GetAllCategoriesBudget(accountId);
                Account account = new Account(accountId, Incomes, Expenses, Categories, CategoryBudget);
                return account;
            }
            catch (Exception ex)
            {
                return new Account();

            }

        }
        public static bool InsertAccount(Account account)
        {
            try
            {
                DAL.InsertAccount(account);
                DAL.InsertIncomes(account.incomes, account);
                DAL.InsertExpenses(account.expenses, account);
                DAL.InsertCategoriesBudget(account.CategoryBudget, account);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
