namespace PersonalFinanceTracker.Interfaces
{
    public interface IRepository
    {
        void SaveAccount(Account account);
        Account GetAccount();

    }

}
