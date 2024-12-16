# Personal Finance Tracker

## Project Overview
**Personal Finance Tracker** is a simple console-based application designed to help users manage their incomes, expenses, and budgets across different spending categories. It enables users to track their financial activities, update existing transactions, and monitor spending to avoid budget overruns.

---

## Features

- **Track Incomes and Expenses**:  
  Add, delete, or update income and expense records.

- **Budget Monitoring**:  
  Set budgets for categories and receive notifications when approaching or exceeding the budget.

- **Balance Calculation**:  
  Automatically updates the account balance based on total incomes and expenses.

- **Category-Based Organization**:  
  Group transactions under specific categories and manage individual budgets for each.

---

## Requirements

### Prerequisites
- **.NET SDK** (Minimum version 6.0)
- A C# compatible environment (e.g., Visual Studio or Visual Studio Code).

---

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yousefsayedd1/-Personal-Finance-Tracker.git
   ```
2. Navigate to the project directory:
   ```bash
    cd Personal-Finance-Tracker
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the Project
   ```bash
   dotnet run
   ```
---
## Installation
1. Add Income or Expense
    - Choose the option to add a new income or expense.
    - Provide details such as the amount, source, date, and category.
2. Update/Delete Transactions
    - Select the transaction number to edit or delete it.
3. Set Category Budgets
    - Add a budget for a specific category.
    - The app will notify you if your spending in that category approaches the budget limit.
4. Display Transactions
    - View all recorded incomes and expenses.
6. Balance Overview
   - The balance is updated automatically after every transaction.
## Project Structure
``` plaintext
Personal-Finance-Tracker/
│
├── Interfaces/
│   └── IUserInterface.cs          # Interface for user interaction methods.
│
├── Models/
│   ├── Income.cs                  # Represents an income transaction.
│   ├── Expense.cs                 # Represents an expense transaction.
│   ├── Transaction.cs             # Base class for all transactions.
│   └── BudgetEventArgs.cs         # Custom event args for budget notifications.
│
├── Account.cs                     # Core class for managing accounts, transactions, and budgets.
│
├── Program.cs                     # Entry point of the application.
│
└── README.md                      # Documentation file.
```
---
## Key Classes
1. ```Account```<br />
Manages:
  - Incomes and expenses lists.
  - Budget settings for categories.
  - Balance calculations and updates.
  - Event notifications for budget overruns.
2. ```BudgetEventArgs```<br />
Custom ```EventArgs``` for notifying when the budget limit is reached.
3. ```IUserInterface```<br />
An interface to define user interaction methods such as:
  - Displaying messages.
  - Reading inputs.
---
## Events and Notifications
The ```Account``` class uses an event to notify users when their spending approaches or exceeds a category budget:
```csharp
public event EventHandler<BudgetEventArgs> BudgetExceeded;
```
Example usage in the code:

```csharp
BudgetExceeded?.Invoke(this, new BudgetEventArgs
{
    TotalExpenses = totalSpent,
    Budget = budget,
    CategoryName = category
});
```
---
## Future Enhancements
1. Persistent Storage: Save and load transactions using files or a database.
2. Improved User Interface: Add a graphical interface (GUI) for better usability.
3. Reports and Analytics: Generate financial summaries and spending trends.
---
## Contribution Guidelines
Contributions are welcome! Here's how you can help:
1. Fork the repository.
2. Create a new branch: ```git checkout -b feature/your-feature```.
3. Commit your changes: ```git commit -m "Add some feature"```.
4. Push to the branch: ```git push origin feature/your-feature```.
5. Open a pull request.




نسخ الكود

نسخ الكود






