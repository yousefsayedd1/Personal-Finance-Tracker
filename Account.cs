using PersonalFinanceTracker.Interfaces;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace PersonalFinanceTracker
{
    public class BudgetEventArgs : EventArgs
    {
        public decimal TotalExpenses;
        public decimal buget;
        public string categoryName;

    }
    public class Account
    {
        public IUserInterface _userInterface  ;
        public event Action<BudgetEventArgs> budgetExceeded;

        public Account() {
            budgetExceeded += BudgetExceededNotify;
        }
        public Account(IUserInterface userInterface) : this()
        {
            _userInterface = userInterface;
        }

        [JsonInclude]
        [JsonPropertyName("Incomes")]
        private List<Income> _incomes = new List<Income>();
        [JsonIgnore]
        public IEnumerable<Income> incomes => _incomes;

        [JsonInclude]
        [JsonPropertyName("Expenses")]
        private List<Expense> _expenses = new List<Expense>();
        [JsonIgnore]
        public IEnumerable<Expense> expenses => _expenses;
        [JsonInclude]
        public List<string> categorys = new List<string>() {};

        [JsonInclude]
        Dictionary<string, KeyValuePair<decimal,decimal>> category_budget = new Dictionary<string, KeyValuePair<decimal, decimal>>();

        [JsonInclude]
        public decimal Balance { get; private set; } = 0;

        [JsonInclude]
        public decimal TotalIncomes { get; private set; } = 0;

        [JsonInclude]
        public decimal TotalExpenses { get; private set; } = 0;

        public void DisplayTransactions(IEnumerable<Transaction> transactions) 
        {
            _userInterface.DisplayTransactions(transactions);
        }
        public void AddIncome(Income income)
        {
            _incomes.Add(income);
            if (!categorys.Contains(income.Category))
            {
                categorys.Add(income.Category);
            }
            updateBalance();
            updateTotalIncomes();
        }
     public void DeleteIncome(int incomeNumber)
        {
            _incomes.Remove(_incomes[incomeNumber-1]);
            updateTotalIncomes();

        }
        public void UpdateIncome(int incomeNumber)
        {
            incomeNumber--;
            Income income = _incomes[incomeNumber];

            _userInterface.DisplayMessage("Enter new Amount:");
            decimal newAmount = _userInterface.ReadDecimal(0);
            _userInterface.DisplayMessage("Enter new Source:");
            string newSource = _userInterface.ReadString();
            DateTime newDate = _userInterface.ReadDate();
            _incomes[incomeNumber] = new Income(newAmount, newSource, newDate);
            updateBalance();
            updateTotalIncomes();

        }
        public void AddExpense(Expense expense)
        {
            _expenses.Add(expense);
            updateBalance();
            updateTotalExpenses();
            if (category_budget.ContainsKey( expense.Category))
            {
                category_budget[expense.Category] = new KeyValuePair<decimal, decimal>(category_budget[expense.Category].Key + expense.Amount, category_budget[expense.Category].Value);
                if (category_budget[expense.Category].Key + 100 >= category_budget[expense.Category].Value) budgetExceeded?.Invoke(new BudgetEventArgs() { TotalExpenses = category_budget[expense.Category].Key, buget = category_budget[expense.Category].Value, categoryName = expense.Category });

            }
        }
        private  void updateTotalExpenses()
        {
            TotalExpenses = _expenses.Sum(expense => expense.Amount);

        }
        private void updateTotalIncomes()
        {
            TotalIncomes = _incomes.Sum(income => income.Amount);

        }

        public void UpdateExpense(int expenseNumber)
        {
            expenseNumber--;
            Expense expense = _expenses[expenseNumber];
            decimal oldAmount = expense.Amount;
            _userInterface.DisplayMessage("Enter new Amount:");
            decimal newAmount = _userInterface.ReadDecimal(0);
            _userInterface.DisplayMessage("Enter new Source:");
            string newSource = _userInterface.ReadString();
            DateTime newDate = _userInterface.ReadDate();
            _expenses[expenseNumber] = new Expense(newAmount, newSource, newDate);
            updateBalance();
            updateTotalExpenses();
            if (category_budget.ContainsKey(expense.Category))
            {
                category_budget[expense.Category] = new KeyValuePair<decimal, decimal>(category_budget[expense.Category].Key + (newAmount - oldAmount), category_budget[expense.Category].Value);
                if (category_budget[expense.Category].Key + 100 >= category_budget[expense.Category].Value) budgetExceeded?.Invoke(new BudgetEventArgs() { TotalExpenses = category_budget[expense.Category].Key, buget = category_budget[expense.Category].Value, categoryName = expense.Category });
            }

        }
        public void DeleteExpense(int ExpenseNumber)
        {
            Expense expense = _expenses[ExpenseNumber - 1];
            _expenses.Remove(expense);
            updateTotalExpenses();
            if (category_budget.ContainsKey(expense.Category))
            {
                category_budget[expense.Category] = new KeyValuePair<decimal, decimal>(category_budget[expense.Category].Key - expense.Amount, category_budget[expense.Category].Value);
                if (category_budget[expense.Category].Key + 100 >= category_budget[expense.Category].Value) budgetExceeded?.Invoke(new BudgetEventArgs() { TotalExpenses = category_budget[expense.Category].Key, buget = category_budget[expense.Category].Value, categoryName = expense.Category });
            }

        }

        public void AddCategoryBudget()
        {
            _userInterface.DisplayMessage("Enter Category Name");
            string categoryName = _userInterface.ReadString();
            while (!categorys.Contains(categoryName))
            {
                _userInterface.DisplayMessage("there is no category with this Name");
                categoryName = _userInterface.ReadString();
            }
            _userInterface.DisplayMessage("Enter Budget:");
            decimal budget = _userInterface.ReadDecimal();
            category_budget[categoryName] = new KeyValuePair<decimal, decimal>(0, budget);

        }
        private void updateBalance()
        {
            Balance = _incomes.Sum(Income => Income.Amount) - _expenses.Sum(Expense => Expense.Amount);
        }
        private async void BudgetExceededNotify(BudgetEventArgs args)
        {

             Task.Run(()=> _userInterface.DisplayMessage($"you are near or Exceeded you have spend {args.TotalExpenses} of the Categroy {args.categoryName} with Buget {args.buget} for this category"));
        }
    }
}
