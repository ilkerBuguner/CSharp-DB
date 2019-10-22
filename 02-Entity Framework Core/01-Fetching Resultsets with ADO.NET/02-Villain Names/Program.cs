using System;
using System.Data.SqlClient;

namespace _02_Villain_Names
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection dbCon = new SqlConnection("Server=DESKTOP-7BEBOBG\\SQLEXPRESS;" +
                "Database=MinionsDB;" +
                "Integrated Security=true");

            dbCon.Open();

            using (dbCon)
            {
                SqlCommand command = new SqlCommand(" SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount" +
                                                    " FROM Villains AS v" +
                                                    " JOIN MinionsVillains AS mv ON v.Id = mv.VillainId" +
                                                    " GROUP BY v.Id, v.Name" +
                                                    " HAVING COUNT(mv.VillainId) > 3" +
                                                    " ORDER BY COUNT(mv.VillainId) DESC", dbCon);

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        string villainName = (string)reader["Name"];
                        int numOfMinions = (int)reader["MinionsCount"];
                        Console.WriteLine(villainName + " - " + numOfMinions);
                    }
                }
            }
        }
    }
}
