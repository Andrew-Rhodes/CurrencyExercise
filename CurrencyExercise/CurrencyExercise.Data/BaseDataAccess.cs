using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;

namespace CurrencyExercise.Data
{
    public class BaseDataAccess : IDisposable
    {
        #region Constructor

        private string _connectionName;

        public BaseDataAccess(string connectionName = "currency")
        {
            _connectionName = connectionName;
            Connection = new SQLiteConnection(ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString);
            Connection.Open();
        }

        #endregion

        #region SQLite helpers

        protected SQLiteConnection Connection { get; private set; }

        protected List<Dictionary<string, object>> ExecuteQuery(string sql)
        {
            var results = new List<Dictionary<string, object>>();
            
            using (var cmd = new SQLiteCommand(sql, Connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var value = reader.GetValue(i);

                        row[columnName] = value;
                    }

                    results.Add(row);
                }
            }

            return results;
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && Connection != null)
                {
                    Connection.Dispose();
                }

                Connection = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}
