using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace CurrencyExercise.Data
{
    public class CurrencyDataAccess : BaseDataAccess
    {
        public CurrencyDataAccess(string connectionName = "currency")
            : base(connectionName)
        {
        }

        public IList<Currency> GetCurrencies()
        {
            var records = ExecuteQuery("SELECT * FROM Currencies");

            return records.Select(row => new Currency
            {
                CurrencyId = (long)row["CurrencyID"],
                CurrencyCode = (string)row["CurrencyCode"],
                CurrencyName = (string)row["CurrencyName"],
                ExchangeRate = (decimal)row["ExchangeRate"],
                LastUpdatedTime = DateTime.Parse((string)row["LastUpdatedTime"])
            }).ToList();
        }

        private long FindCurrencyIdByCode(string code)
        {
            var records = ExecuteQuery($"SELECT CurrencyID FROM Currencies WHERE CurrencyCode = '{code}'");

            if (records.Count == 1)
            {
                return (long)records[0]["CurrencyID"];
            }
            else
            {
                return -1;
            }
        }

        public long SaveCurrency(Currency currency)
        {
            long currencyId = FindCurrencyIdByCode(currency.CurrencyCode);

            SQLiteCommand command;
            
            if (currencyId != -1)
            {                
                command = new SQLiteCommand(
                    "UPDATE Currencies " +
                        "SET ExchangeRate = @ExchangeRate, LastUpdatedTime = @LastUpdatedTime " +
                        "WHERE CurrencyID = @CurrencyID AND ExchangeRate != @ExchangeRate", 
                    Connection);

                command.Parameters.AddWithValue("@ExchangeRate", currency.ExchangeRate);
                command.Parameters.AddWithValue("@LastUpdatedTime", DateTime.Now.ToString("O"));
                command.Parameters.AddWithValue("@CurrencyID", currencyId);
                
                command.ExecuteNonQuery();
            }
            else
            {
                command = new SQLiteCommand(
                    "INSERT INTO Currencies (CurrencyCode, CurrencyName, ExchangeRate, LastUpdatedTime)" +
                        "VALUES (@CurrencyCode, @CurrencyName, @ExchangeRate, @LastUpdatedTime)",
                    Connection);

                command.Parameters.AddWithValue("@CurrencyCode", currency.CurrencyCode);
                command.Parameters.AddWithValue("@CurrencyName", currency.CurrencyName);
                command.Parameters.AddWithValue("@ExchangeRate", currency.ExchangeRate);
                command.Parameters.AddWithValue("@LastUpdatedTime", DateTime.Now.ToString("O"));

                command.ExecuteNonQuery();

                currencyId = FindCurrencyIdByCode(currency.CurrencyCode);
            }
            
            return currencyId;
        }
    }
}
