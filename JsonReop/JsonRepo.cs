﻿using System.Text.Json;
using PersonalFinanceTracker.Interfaces;

namespace PersonalFinanceTracker.JsonReop
{
    public class JsonRepo : IRepository
    {
        string FilePath = "Accounts.json";
        public Account GetAccountById(string accountId)
        {

            string StringFileContent = File.ReadAllText(FilePath);
            try
            {
                var options = new JsonSerializerOptions { IncludeFields = true };

                Account account = JsonSerializer.Deserialize<Account>(StringFileContent, options);
                return account;
            }
            catch (JsonException ex)
            {
                return new Account();
            }
        }

        public void SaveAccount(Account account)
        {
            string jsonString = JsonSerializer.Serialize(account);
            File.WriteAllText(FilePath, jsonString);
        }
    }

}
