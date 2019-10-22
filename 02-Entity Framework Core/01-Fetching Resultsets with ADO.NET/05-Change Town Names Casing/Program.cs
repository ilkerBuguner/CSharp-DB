using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _05_Change_Town_Names_Casing
{
    public class Program
    {
        private static string ConnectionString = "Server=DESKTOP-7BEBOBG\\SQLEXPRESS;" +
            "Database=MinionsDB;" +
            "Integrated Security=true";

        private const string EditTownNames = @"UPDATE Towns
                                               SET Name = UPPER(Name)
                                               WHERE CountryCode = (
                                               SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

        private const string FindEditedTowns = @"SELECT t.Name 
                                               FROM Towns as t
                                               JOIN Countries AS c ON c.Id = t.CountryCode
                                               WHERE c.Name = @countryName";

        static void Main(string[] args)
        {
            string country = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(EditTownNames, connection))
                {
                    command.Parameters.AddWithValue("@countryName", country);
                    int count = command.ExecuteNonQuery();

                    Console.WriteLine($"{count} town names were affected.");
                }

                using (SqlCommand command = new SqlCommand(FindEditedTowns, connection))
                {
                    command.Parameters.AddWithValue("@countryName", country);

                    List<string> cities = new List<string>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cities.Add((string)reader["Name"]);
                        }
                    }

                    if (cities.Count == 0)
                    {
                        Console.WriteLine($"No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"[{string.Join(", ", cities)}]");
                    }
                }
            }
        }
    }
}
