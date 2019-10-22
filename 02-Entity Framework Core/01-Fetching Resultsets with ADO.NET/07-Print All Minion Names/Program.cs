using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _07_Print_All_Minion_Names
{
    class Program
    {
        private const string ConnectionString = "Server=DESKTOP-7BEBOBG\\SQLEXPRESS;" +
                                                "Database=MinionsDB;" +
                                                "Integrated Security=true";

        private const string FindMinionNames = "SELECT Name FROM Minions";
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(FindMinionNames, connection))
                {
                    List<string> minions = new List<string>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minions.Add((string)reader["Name"]);
                        }
                    }

                    for (int i = 0; i < minions.Count / 2; i++)
                    {
                        Console.WriteLine(minions[i]);
                        Console.WriteLine(minions[minions.Count - i - 1]);
                    }
                }
            }
        }
    }
}
