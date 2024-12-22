using System;
using System.Data.Common;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PersonalFinanceTracker.Interfaces

{
    public interface IRepository
    {
        void SaveAccount(Account account);
        Account GetAccountById(string accountId);

    }

}
