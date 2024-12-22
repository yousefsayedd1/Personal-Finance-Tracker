using PersonalFinanceTracker.Interfaces;
using System;

namespace PersonalFinanceTracker.UserInterfaces
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void DisplayMenu()
        {
            Console.WriteLine("1. Add an Income");
            Console.WriteLine("2. List Incoms");
            Console.WriteLine("3. Update Income");
            Console.WriteLine("4. Delete Income");
            Console.WriteLine("5. Add Expense");
            Console.WriteLine("6. List Expenses");
            Console.WriteLine("7. Update Expense");
            Console.WriteLine("8. Delete Expense");
            Console.WriteLine("9. Set Category Budget");
            Console.WriteLine("10. General Report");
            Console.WriteLine("11. Date Based Report");
        }


        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public Income ReadIncomeFromUser()
        {
            DisplayMessage("Enter Amount:");
            decimal Amount = ReadDecimal();
            DisplayMessage("Enter Category:");

            string Source = ReadString();
            DateTime date = ReadDate();
            return new Income(Amount, Source, date);

        }
        public Expense ReadExpenceFromUser()
        {
            DisplayMessage("Enter Amount:");
            decimal Amount = ReadDecimal();
            DisplayMessage("Enter Category:");

            string Source = ReadString();
            DateTime date = ReadDate();
            return new Expense(Amount, Source, date);

        }
        public int ReadInt(int min = int.MinValue, int max = int.MaxValue)
        {
            int result;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out result))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    continue;
                }

                if (result < min || result > max)
                {
                    Console.WriteLine($"Number out of range. Please enter a number between {min} and {max}.");
                    continue;
                }
                return result;
            }

        }
        public string ReadString()
        {

            return Console.ReadLine();
        }
        public decimal ReadDecimal(decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
        {
            decimal result;
            while (true)
            {
                if (!decimal.TryParse(Console.ReadLine(), out result))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    continue;
                }

                if (result < min || result > max)
                {
                    Console.WriteLine($"Number out of range. Please enter a number between {min} and {max}.");
                    continue;
                }
                return result;
            }
        }
        public void DisplayTransactions(IEnumerable<Transaction> transactions)
        {
            int i = 1;
            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine($"{i}. {transaction.ToString()}");
                i++;
            }
        }
        public DateTime ReadDate()
        {
            DateTime date;
            while (true)
            {
                try
                {
                    DisplayMessage("Enter Day");
                    int day = ReadInt();
                    DisplayMessage("Enter Month");
                    int Month = ReadInt();
                    DisplayMessage("Enter Year");
                    int Year = ReadInt();
                    if (Year > DateTime.Now.Year) throw new Exception();
                    date = new DateTime(Year, Month, day);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Day must be in range 1 to (30,31), Month Must be in range (1,12), Year must be current year or a past year {ex.Message}");
                }

            }
            return date;
        }

        public void DisplayCategories(List<string> categories)
        {
            foreach (string category in categories)
            {
                Console.WriteLine(category);
            }
        }
    }
}
