using WebAPIContosoPizzaVS.Models;
using Microsoft.Data.Sqlite;

namespace WebAPIContosoPizzaVS.Services
{
    public static class PizzaServiceDAL
    {
        private static readonly DbHelper dbHelper = new();

        static PizzaServiceDAL() { }

        public static List<Pizza> GetAll()
        {
            List<Pizza> pizzaList = new();
            using (SqliteConnection physicalDbConnection = dbHelper.GetPhysicalDbConnection())
            using (SqliteConnection inMemoryDbConnection = dbHelper.GetInMemoryDbConnection())
            {
                physicalDbConnection.Open();
                inMemoryDbConnection.Open();

                // 1. Duplication physical database to in-memory
                physicalDbConnection.BackupDatabase(inMemoryDbConnection, "main", "main");

                // 2. Operations on the in-memory database
                string query = $"SELECT * FROM PizzaType";
                using (SqliteCommand command = new())
                {
                    command.CommandText = query;
                    command.Connection = inMemoryDbConnection;
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pizzaList.Add(new Pizza() { Id = Convert.ToInt32(reader["Id"]), Name = Convert.ToString(reader["Name"]), IsGlutenFree = Convert.ToBoolean(reader["IsGlutenFree"]) });
                        }
                    }
                }
            }
            return pizzaList;
        }

        public static Pizza? Get(int id)
        {
            Pizza? pizza;
            List<Pizza> pizzaList = GetAll();
            pizza = pizzaList.FirstOrDefault(pizza => pizza.Id == id);
            return pizza;
        }

        public static void Add(Pizza pizza)
        {
            using (SqliteConnection physicalDbConnection = dbHelper.GetPhysicalDbConnection())
            using (SqliteConnection inMemoryDbConnection = dbHelper.GetInMemoryDbConnection())
            {
                physicalDbConnection.Open();
                inMemoryDbConnection.Open();

                // 1. Duplication physical database to in-memory
                physicalDbConnection.BackupDatabase(inMemoryDbConnection, "main", "main");

                // 2. Operations on the in-memory database
                string query = $"INSERT INTO PizzaType(Id, Name, IsGlutenFree) VALUES('{pizza.Id}', '{pizza.Name}', '{pizza.IsGlutenFree}')";
                using (SqliteCommand command = new())
                {
                    command.CommandText = query;
                    command.Connection = inMemoryDbConnection;
                    command.ExecuteNonQuery();
                }

                // 3. Backup the in-memory database to the physical database
                inMemoryDbConnection.BackupDatabase(physicalDbConnection);
            }
        }

        public static void Delete(int id)
        {
            Pizza? pizza = Get(id);
            if (pizza is not null)
            {
                using (SqliteConnection physicalDbConnection = dbHelper.GetPhysicalDbConnection())
                using (SqliteConnection inMemoryDbConnection = dbHelper.GetInMemoryDbConnection())
                {
                    physicalDbConnection.Open();
                    inMemoryDbConnection.Open();

                    physicalDbConnection.BackupDatabase(inMemoryDbConnection, "main", "main");

                    string query = $"DELETE FROM PizzaType WHERE id = '{pizza.Id}'";
                    using (SqliteCommand command = new(query, inMemoryDbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    inMemoryDbConnection.BackupDatabase(physicalDbConnection);
                }
            }
        }

        public static void Update(Pizza updatedPizza)
        {
            Pizza? pizza = Get(updatedPizza.Id);
            if (pizza is not null)
            {
                using (SqliteConnection physicalDbConnection = dbHelper.GetPhysicalDbConnection())
                using (SqliteConnection inMemoryDbConnection = dbHelper.GetInMemoryDbConnection())
                {
                    physicalDbConnection.Open();
                    inMemoryDbConnection.Open();

                    physicalDbConnection.BackupDatabase(inMemoryDbConnection, "main", "main");

                    string query = $"UPDATE PizzaType SET Name = '{updatedPizza.Name}', IsGlutenFree = '{updatedPizza.IsGlutenFree}' WHERE Id = '{updatedPizza.Id}'";
                    using (SqliteCommand command = new(query, inMemoryDbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                    inMemoryDbConnection.BackupDatabase(physicalDbConnection);
                }
            }
        }
    }
}