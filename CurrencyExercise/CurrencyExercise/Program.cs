using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CurrencyExercise.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CurrencyExercise
{
    public class Program
    {
        /// <summary>
        /// This program loads the latest currency exchange rate data from openexchangerates.org.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CurrencyDataAccess CDA = new CurrencyDataAccess();
            List<Currency> currencyList = new List<Currency>();
            Dictionary<string, string> names = GetCurrencyNames();
            Dictionary<string, decimal> rates = GetExchangeRates();

            currencyList = AddNames(names, currencyList);
            currencyList = AddRates(rates, currencyList);

            using (var currencyData = new CurrencyDataAccess())
            {
                foreach (var currency in currencyList)
                {
                    CDA.SaveCurrency(currency);
                }
            }
        }

        private static List<Currency> AddRates(Dictionary<string, decimal> rates, List<Currency> currencyList)
        {
            decimal hasKey = 0;
            foreach (var cur in currencyList)
            {
                rates.TryGetValue(cur.CurrencyCode, out hasKey);

                if (hasKey > 0)
                {
                    cur.ExchangeRate = rates[cur.CurrencyCode];
                }
                else
                {
                    cur.ExchangeRate = 0;
                }
            }

            return currencyList;
        }

        private static List<Currency> AddNames(Dictionary<string, string> names, List<Currency> currencyList)
        {
            foreach(var name in names)
            {
                Currency currencyToAdd = new Currency();

                currencyToAdd.CurrencyName = name.Value;
                currencyToAdd.CurrencyCode = name.Key;

                currencyList.Add(currencyToAdd);
            }

            return currencyList;
        }

        private static Dictionary<string, decimal> GetExchangeRates()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["CurrencyUrl"]);
            ResponseModel rm = new ResponseModel();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(@"\api\latest.json?app_id=" + ConfigurationManager.AppSettings["OpenExchangeRates.ApiKey"]).Result;
            if (response.IsSuccessStatusCode)
            {
                rm = response.Content.ReadAsAsync<ResponseModel>().Result;
            }

            client.Dispose();

            return rm.Rates;
        }


        /// <summary>
        /// This method provides a dictionary that can be used to find the currency name
        /// which corresponds to a currency code (e.g. USD code has the name "United States Dollar").        
        /// </summary>
        /// <returns>A dictionary where for each entry the key is the currency code and the value is the corresponding name.</returns>
        public static Dictionary<string, string> GetCurrencyNames()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["CurrencyUrl"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Dictionary<string, string> names = new Dictionary<string, string>();

            HttpResponseMessage response = client.GetAsync(@"/api/currencies.json?app_id=" + ConfigurationManager.AppSettings["OpenExchangeRates.ApiKey"]).Result;
            if (response.IsSuccessStatusCode)
            {
                names = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;
            }

            client.Dispose();

            return names;
        }
    }
}




//These are my queries for the second part of the assessment. 
//I will also attatch a link to a Google Doc for my results in my follow up email. 
//Thank you

//SELECT* FROM Currencies

//SELECT* FROM Orders

//SELECT C.CurrencyName, C.CurrencyCode, C.ExchangeRate, O.PaymentAmount, O.PaymentAmount / C.ExchangeRate AS AmountInUSD FROM Orders O
//LEFT JOIN Currencies C
//ON O.CurrencyCode = C.CurrencyCode
//GROUP BY C.CurrencyCode
//ORDER BY AmountInUSD DESC

