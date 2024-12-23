using System.Data;
using System.Data.SqlClient;


namespace PersonalFinanceTracker.AdoSqlRepo

{
    public static class DAL
    {
        static SqlConnection sqlConnection = new SqlConnection("Data Source=.;Initial Catalog=PersonalFinanceTracker;Integrated Security=True;");
        static SqlCommand sqlCommand = new SqlCommand();
        static SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        static DAL()
        {
            sqlCommand.Connection = sqlConnection;
            sqlDataAdapter.SelectCommand = sqlCommand;
        }
        public static DataTable GetAllAccountIncomes(string accountId)
        {
            try
            {
                sqlCommand.CommandText = $"SELECT * FROM Income WHERE Account_ID = '{accountId}'";
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {
                throw;
            }
        }
        public static DataTable GetAllAccountExpenses(string accountId)
        {
            try
            {
                sqlCommand.CommandText = $"SELECT * FROM Expense WHERE Account_ID = '{accountId}'";
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch
            {

                throw;
            }
        }
        public static DataTable GetAllCategoriesByAccountId(string accountId)
        {
            sqlCommand.CommandText = $"SELECT Name FROM Category WHERE AccID = '{accountId}'";
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public static DataTable GetAllCategoriesBudgetByAccountId(string accountId)
        {
            sqlCommand.CommandText = $"SELECT Name, Budget FROM Category WHERE AccID = '{accountId}'";
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public static bool InsertAccount(Account account)
        {
            try
            {
                sqlConnection.Open();
                sqlCommand.CommandText = $"insert into Account values ('{account.ID}')";
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                sqlCommand.CommandText = $"update Account set ID = '{account.ID}' where  ID = '{account.ID}'";
                sqlCommand.ExecuteNonQuery();

            }
            finally
            {
                sqlConnection.Close();
            }
            return true;
        }
        public static bool InsertIncomes(IEnumerable<Income> incomes, Account account)
        {
            try
            {
                sqlConnection.Open();
                DataTable dataTable = GetAllAccountIncomes(account.ID);
                foreach (DataRow row in dataTable.Rows)
                {
                    Income income = new Income(row["Id"]?.ToString(), Convert.ToDecimal(row["Amount"]), row["Category"]?.ToString(), Convert.ToDateTime(row["Date"]));
                    if (!incomes.Contains(income))
                    {
                        sqlCommand.CommandText = $"delete from Income where ID = '{income.Id}'";
                        sqlCommand.ExecuteNonQuery();

                    }
                }
                foreach (var income in incomes)
                {
                    try
                    {
                        sqlCommand.CommandText = $"insert into Income (ID,Amount, Category, Date, Account_ID) values ('{income.Id}',{income.Amount}, '{income.Category}', '{income.Date}' , '{account.ID}')";
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        sqlCommand.CommandText = $"update Income set Amount = {income.Amount}, Category = '{income.Category}', Date = '{income.Date}' where ID = '{income.Id}'";
                        sqlCommand.ExecuteNonQuery();
                    }
                    try
                    {
                        sqlCommand.CommandText = $"insert into category values ('{income.Category}',Null,'{account.ID}')";
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch { }

                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return true;

        }
        public static bool InsertExpenses(IEnumerable<Expense> expenses, Account account)
        {
            try
            {
                sqlConnection.Open();
                DataTable dataTable = GetAllAccountExpenses(account.ID);
                foreach (DataRow row in dataTable.Rows)
                {
                    Expense expense = new Expense(row["Id"]?.ToString(), Convert.ToDecimal(row["Amount"]), row["Category"]?.ToString(), Convert.ToDateTime(row["Date"]));
                    if (!expenses.Contains(expense))
                    {
                        sqlCommand.CommandText = $"delete from Expense where ID = '{expense.Id}'";
                        sqlCommand.ExecuteNonQuery();

                    }
                }

                foreach (var expense in expenses)
                {
                    try
                    {
                        sqlCommand.CommandText = $"insert into Expense (ID,Amount, Category, Date, Account_ID) values ('{expense.Id}',{expense.Amount}, '{expense.Category}', '{expense.Date}', '{account.ID}')";
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch
                    {

                        sqlCommand.CommandText = $"update Expense set Amount = {expense.Amount}, Category = '{expense.Category}', Date = '{expense.Date}' where ID = '{expense.Id}'";
                        sqlCommand.ExecuteNonQuery();
                    }
                    try
                    {
                        sqlCommand.CommandText = $"insert into category values ('{expense.Category}',Null,'{account.ID}')";
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch { }
                }
                return true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sqlConnection.Close();
            }
            return true;

        }
        public static bool InsertCategoriesBudget(Dictionary<string, KeyValuePair<decimal, decimal>> categroiesBudget, Account account)
        {
            try
            {
                sqlConnection.Open();

                foreach (var category in categroiesBudget)
                {
                    sqlCommand.CommandText = $"update Category set Budget = {category.Value.Value} where name = '{category.Key}' and AccID = '{account.ID}'";
                    sqlCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }

}
