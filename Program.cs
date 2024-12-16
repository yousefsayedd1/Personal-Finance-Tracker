using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Principal;
using PersonalFinanceTracker.Interfaces;

namespace PersonalFinanceTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IUserInterface _userInterface = new ConsoleUserInterface();
            IRepository _jsonRepo = new JsonRepo();
            App app = new App(_userInterface, _jsonRepo, new Account(_userInterface));
            app.Run();

        }
    }
    public class App 
    {
        private IUserInterface _userInterface;
        private IRepository _repository;
        private Account _userAccount;
        public App(IUserInterface userInterface, IRepository repository, Account userAccount)
        {
            _userInterface = userInterface;
            
            _repository = repository;
            _userAccount = userAccount;
        }
        public void Run()
        {
            // task: add validation on date input
            int numberOfChoices = 11;
            _userAccount =  _repository.GetAccount();
            _userAccount._userInterface = _userInterface;
            while (true) {
            int numberOfIncomes = _userAccount.incomes.Count();
            int numberOfExpense = _userAccount.expenses.Count();
               
                _userInterface.DisplayMenu();
                _userInterface.DisplayMessage("Pick a number:");
                int choice = _userInterface.ReadInt(1,numberOfChoices);
                UserNeed userChoice = (UserNeed)choice;
                switch (userChoice)
                {
                    case UserNeed.AddIncome:
                        Income income = _userInterface.ReadIncomeFromUser();
                        _userAccount.AddIncome(income);
                        break;
                    case UserNeed.DisplayIncomes:
                        _userInterface.DisplayMessage("Incomes");
                        _userAccount.DisplayTransactions(_userAccount.incomes);
                        break;
                    case UserNeed.UpdateIncome:
                        _userAccount.DisplayTransactions(_userAccount.incomes);
                        _userInterface.DisplayMessage("Pick the Income Number you want to update:");
                        int incomeNumber = _userInterface.ReadInt(1,numberOfIncomes);
                        _userAccount.UpdateIncome(incomeNumber);
                        break;
                    case UserNeed.DeleteIncome:
                        _userAccount.DisplayTransactions(_userAccount.incomes);
                        _userInterface.DisplayMessage("Pick the Income Number you want to update:");
                        incomeNumber = _userInterface.ReadInt(1, numberOfIncomes);
                        _userAccount.DeleteIncome(incomeNumber);
                        break;
                    case UserNeed.AddExpense:
                        Expense expense = _userInterface.ReadExpenceFromUser();
                        _userAccount.AddExpense(expense);
                        break;
                    case UserNeed.DisplayExpense:
                        _userInterface.DisplayMessage("Expenses");
                        _userAccount.DisplayTransactions(_userAccount.expenses);
                        break;
                    case UserNeed.UpdateExpense:
                        _userAccount.DisplayTransactions(_userAccount.expenses);
                        _userInterface.DisplayMessage("Pick the Expense Number you want to update:");
                        int expenseNumber = _userInterface.ReadInt(1, numberOfExpense);
                        _userAccount.UpdateExpense(expenseNumber);
                        break;
                    case UserNeed.DeleteExpense:
                        _userAccount.DisplayTransactions(_userAccount.expenses);
                        _userInterface.DisplayMessage("Pick the Expense Number you want to update:");
                        expenseNumber = _userInterface.ReadInt(1, numberOfExpense);
                        _userAccount.DeleteExpense(expenseNumber);
                        break;
                    case UserNeed.SetBudget:
                        _userAccount.AddCategoryBudget();
                        break;
                    case UserNeed.GeneralReport:
                        _userInterface.DisplayMessage(Reporting.GeneralReport(_userAccount)); 
                        break;
                    case UserNeed.DateBasedReport:
                        _userInterface.DisplayMessage("Enter Start Date:");
                        DateTime startDate = _userInterface.ReadDate();
                        _userInterface.DisplayMessage("Enter End Date:");
                        DateTime endDate = _userInterface.ReadDate();
                        _userInterface.DisplayMessage("Enter Categroy:");
                        string category = _userInterface.ReadString();
                        _userInterface.DisplayMessage(Reporting.ActivityDateBasedReport(_userAccount, category, startDate, endDate));
                        break;
                    default:
                        _userInterface.DisplayMessage("Invalid choice. Please select a valid option.");
                        break;
                }
                _userInterface.DisplayMessage("Wanna do something more? (Y/N)");
                if (_userInterface.ReadString().ToUpper() == "N")
                {
                    break;
                }
                Console.Clear();
                _repository.SaveAccount(_userAccount);
            }
            
        }
           
    }
    enum UserNeed
    {
        AddIncome = 1,
        DisplayIncomes,
        UpdateIncome,
        DeleteIncome,
        AddExpense,
        DisplayExpense,
        UpdateExpense,
        DeleteExpense,
        SetBudget,
        GeneralReport,
        DateBasedReport,

    }

}

