using PersonalFinanceTracker.EventsArgs;
using PersonalFinanceTracker.Interfaces;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace PersonalFinanceTracker
{
    public class Account
    {
        public IUserInterface _userInterface  ;
        public event Func<BudgetEventArgs, Task> BudgetExceeded;


        public Account() {
            ID = Ulid.NewUlid().ToString();

            BudgetExceeded += BudgetExceededNotify;
        }
        public Account(string id, List<Income> incomes, List<Expense> expenses, List<string> categories, Dictionary<string, KeyValuePair<decimal, decimal>> categoryBudget) 
        {
            ID = id;
            _incomes = incomes;
            _expenses = expenses;
            Categories = categories;
            CategoryBudget = categoryBudget;

            updateCategoryBudget(_expenses);
            UpdateBalance();
            UpdateTotalExpenseAmount();
            UpdateTotalIncomeAmount();
            BudgetExceeded += BudgetExceededNotify;

        }
        [JsonInclude]
        public string ID { get; private set; }
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
        public List<string> Categories { get; private set; } = new List<string>() {};

        [JsonInclude]
        public Dictionary<string, KeyValuePair<decimal,decimal>> CategoryBudget { get; private set; } = new Dictionary<string, KeyValuePair<decimal, decimal>>();

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
        public void Displaycategories()
        {
            _userInterface.DisplayCategories(Categories);
        }
        public void AddIncome(Income income)
        {
            try
            {
                _incomes.Add(income);
                if (!Categories.Contains(income.Category))
                {
                    Categories.Add(income.Category);
                }
                UpdateBalance();
                UpdateTotalIncomeAmount();
            }catch(Exception ex)
            {
                _userInterface.DisplayMessage($"Error Adding income: {ex.Message}");
            }
        }
        public void DeleteIncome(int incomeNumber)
        {
            try
            {
                _incomes.Remove(_incomes[incomeNumber - 1]);
                UpdateTotalIncomeAmount();
            }catch(Exception ex)
            {
                _userInterface.DisplayMessage($"Error Deleting income: {ex.Message}");
            }

        }
        public void UpdateIncome(int incomeNumber)
        {
            try
            {
                incomeNumber--;
                Income income = _incomes[incomeNumber];

                _userInterface.DisplayMessage("Enter new Amount:");
                decimal newAmount = _userInterface.ReadDecimal(0);
                _userInterface.DisplayMessage("Enter new Source:");
                string newSource = _userInterface.ReadString();
                DateTime newDate = _userInterface.ReadDate();
                _incomes[incomeNumber] = new Income(_incomes[incomeNumber].Id,newAmount, newSource, newDate);
                UpdateBalance();
                UpdateTotalIncomeAmount();
            }
            catch (Exception ex)
            {
                _userInterface.DisplayMessage($"Error updating income: {ex.Message}");
            }
        }
        public void AddExpense(Expense expense)
        {
            try
            {
                _expenses.Add(expense);
                UpdateBalance();
                UpdateTotalExpenseAmount();
                if (CategoryBudget.ContainsKey(expense.Category))
                {
                    var currentBudget = CategoryBudget[expense.Category];
                    var updatedExpenses = currentBudget.Key + expense.Amount;
                    CategoryBudget[expense.Category] = new KeyValuePair<decimal, decimal>(updatedExpenses, currentBudget.Value);
                    if (updatedExpenses + 100 >= currentBudget.Value)
                    {
                        BudgetExceeded?.Invoke(new BudgetEventArgs
                        {
                            TotalExpenses = updatedExpenses,
                            budget = currentBudget.Value,
                            categoryName = expense.Category
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _userInterface.DisplayMessage($"Error Adding expense: {ex.Message}");
            }
        }

        public void UpdateExpense(int expenseNumber)
        {
            try
            {
                expenseNumber--;
                Expense expense = _expenses[expenseNumber];
                decimal oldAmount = expense.Amount;
                _userInterface.DisplayMessage("Enter new Amount:");
                decimal newAmount = _userInterface.ReadDecimal(0);
                _userInterface.DisplayMessage("Enter new Source:");
                string newSource = _userInterface.ReadString();
                DateTime newDate = _userInterface.ReadDate();
                _expenses[expenseNumber] = new Expense(_expenses[expenseNumber].Id,newAmount, newSource, newDate);
                UpdateBalance();
                UpdateTotalExpenseAmount();
                if (CategoryBudget.ContainsKey(expense.Category))
                {
                    var currentBudget = CategoryBudget[expense.Category];
                    var updatedExpenses = currentBudget.Key + expense.Amount;
                    CategoryBudget[expense.Category] = new KeyValuePair<decimal, decimal>(updatedExpenses, currentBudget.Value);
                    if (updatedExpenses + 100 >= currentBudget.Value)
                    {
                        BudgetExceeded?.Invoke(new BudgetEventArgs
                        {
                            TotalExpenses = updatedExpenses,
                            budget = currentBudget.Value,
                            categoryName = expense.Category
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _userInterface.DisplayMessage($"Error updating expense: {ex.Message}");
            }

        }
        public void DeleteExpense(int ExpenseNumber)
        {
            try
            {
                Expense expense = _expenses[ExpenseNumber - 1];
                _expenses.Remove(expense);
                UpdateTotalExpenseAmount();
                if (CategoryBudget.ContainsKey(expense.Category))
                {
                    var currentBudget = CategoryBudget[expense.Category];
                    var updatedExpenses = currentBudget.Key - expense.Amount;
                    CategoryBudget[expense.Category] = new KeyValuePair<decimal, decimal>(updatedExpenses, currentBudget.Value);
                    if (updatedExpenses + 100 >= currentBudget.Value)
                    {
                        BudgetExceeded?.Invoke(new BudgetEventArgs
                        {
                            TotalExpenses = updatedExpenses,
                            budget = currentBudget.Value,
                            categoryName = expense.Category
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                _userInterface.DisplayMessage($"Error Deleting expense: {ex.Message}");
            }
        }

        public void AddCategoryBudget()
        {
            _userInterface.DisplayMessage("Enter Category Name");
            string categoryName = _userInterface.ReadString();
            while (!Categories.Contains(categoryName))
            {
                _userInterface.DisplayMessage("there is no category with this Name");
                categoryName = _userInterface.ReadString();
            }
            _userInterface.DisplayMessage("Enter Budget:");
            decimal budget = _userInterface.ReadDecimal();
            CategoryBudget[categoryName] = new KeyValuePair<decimal, decimal>(0, budget);

        }
        private void UpdateBalance()
        {
            Balance = _incomes.Sum(Income => Income.Amount) - _expenses.Sum(Expense => Expense.Amount);
        }
        private void UpdateTotalIncomeAmount()
        {
            TotalIncomes = _incomes.Sum(income => income.Amount);

        }
        private  void UpdateTotalExpenseAmount()
        {
            TotalExpenses = _expenses.Sum(expense => expense.Amount);

        }
        private async Task BudgetExceededNotify(BudgetEventArgs args)
        {

                await Task.Run(()=> _userInterface.DisplayMessage($"you are near or Exceeded you have spend {args.TotalExpenses} of the Categroy {args.categoryName} with Buget {args.budget} for this category"));
                
            
        }
        private void updateCategoryBudget(List<Expense> expenses)
        {
            foreach (Expense expense in expenses)
            {
                if (CategoryBudget.ContainsKey(expense.Category))
                {
                    var currentBudget = CategoryBudget[expense.Category];
                    var updatedExpenses = currentBudget.Key + expense.Amount;
                    CategoryBudget[expense.Category] = new KeyValuePair<decimal, decimal>(updatedExpenses, currentBudget.Value);
                    if (updatedExpenses + 100 >= currentBudget.Value)
                    {
                        BudgetExceeded?.Invoke(new BudgetEventArgs
                        {
                            TotalExpenses = updatedExpenses,
                            budget = currentBudget.Value,
                            categoryName = expense.Category
                        });
                    }
                }
            }
        }
    }
}
