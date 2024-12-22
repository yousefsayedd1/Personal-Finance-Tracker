using PersonalFinanceTracker.Interfaces;


namespace PersonalFinanceTracker.AdoSqlRepo

{
    public class SqlRepository : IRepository
    {
        public void SaveAccount(Account account)
        {
            // Save account to SQL database
            BLL.InsertAccount(account);

        }

        public Account GetAccountById(string accountId)
        {
            // Get account from SQL database
            return BLL.GetAccountById(accountId);
        }
    }

}
