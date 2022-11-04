using Microsoft.Data.Sqlite;
using Oracle.ManagedDataAccess.Client;

namespace WebAPIContosoPizzaVS
{
    public interface IDbHelper
    {
        SqliteConnection GetPhysicalDbConnection();
        SqliteConnection GetInMemoryDbConnection();
    }

    public class DbHelper : IDbHelper
    {
        public readonly string physicalDbFilePath = $@"{Environment.CurrentDirectory}\Sqlite3DB\PizzaRepoDB.db";
        private SqliteConnection inMemoryDbConnection;

        public DbHelper() { }

        public SqliteConnection GetPhysicalDbConnection()
        {
            SqliteConnection dbConnection = new("Data Source =" + physicalDbFilePath + ";Mode=ReadWrite");
            return dbConnection;
        }

        public SqliteConnection GetInMemoryDbConnection()
        {
            if (inMemoryDbConnection == null)
            {
                inMemoryDbConnection = new SqliteConnection("Data Source=:memory:");
            }
            return inMemoryDbConnection;
        }
    }
}
