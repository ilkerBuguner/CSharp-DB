using System;
using System.Data;
using System.Data.SqlClient;

namespace _09_Increase_Age_Stored_Procedure
{
    class Program
    {
        private const string ConnectionString = "Server=DESKTOP-7BEBOBG\\SQLEXPRESS;" +
                                                "Database=MinionsDB;" +
                                                "Integrated Security=true";

        private const string CreateProcedure = @"CREATE PROC usp_GetOlder @id INT
                                                AS
                                                UPDATE Minions
                                                SET Age += 1
                                                WHERE Id = @id";

        private const string SelectMinion = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(CreateProcedure, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("usp_GetOlder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(SelectMinion, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string name = (string)reader[0];
                        int age = (int)reader[1];

                        Console.WriteLine($"{name} - {age} years old");
                    }
                }
            }
        }
    }
}
