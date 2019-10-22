using System;
using System.Data.SqlClient;
using System.Linq;

namespace _08_Increase_Minion_Age
{
    class Program
    {
        private const string ConnectionString = "Server=DESKTOP-7BEBOBG\\SQLEXPRESS;" +
                                                "Database=MinionsDB;" +
                                                "Integrated Security=true";

        private const string EditSelectedMinionNames = @"UPDATE Minions
                                                        SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                        WHERE Id = @Id";

        public const string SelectMinionInformation = "SELECT Name, Age FROM Minions";
        static void Main(string[] args)
        {
            string[] minionsIds = Console.ReadLine().Split().ToArray();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(EditSelectedMinionNames, connection))
                {
                    for (int i = 0; i < minionsIds.Length; i++)
                    {
                        command.Parameters.AddWithValue("@Id", minionsIds[i]);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }

                using (SqlCommand command = new SqlCommand(SelectMinionInformation, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        int age = (int)reader["Age"];

                        Console.WriteLine($"{name} {age}");
                    }
                }
            }
        }
    }
}
