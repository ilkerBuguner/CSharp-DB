using System;
using System.Data.SqlClient;

namespace _03_Minion_Names
{
    public class Program
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            SqlConnection dbCon = new SqlConnection("Server=DESKTOP-7BEBOBG\\SQLEXPRESS;" +
                "Database=MinionsDB;" +
                "Integrated Security=true");

            dbCon.Open();

            using (dbCon)
            {
                SqlCommand commandForVillainCheck = new SqlCommand($" SELECT Name FROM Villains " +
                                                                   $" WHERE Id = {villainId} ", dbCon);

                string villainName = (string)commandForVillainCheck.ExecuteScalar();

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                    return;
                }


                SqlCommand commandForVillainMinions = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum," +
                                         $" m.Name," +
                                         $" m.Age" +
                                         $" FROM MinionsVillains AS mv" +
                                         $" JOIN Minions As m ON mv.MinionId = m.Id" +
                                         $" WHERE mv.VillainId = {villainId}" +
                                         $" ORDER BY m.Name", dbCon);

                SqlDataReader readerForVillainMinions = commandForVillainMinions.ExecuteReader();


                using (readerForVillainMinions)
                {
                    if (!readerForVillainMinions.HasRows)
                    {
                        Console.WriteLine($"Villain: {villainName}");
                        Console.WriteLine("(no minions)");
                    }
                    else if (readerForVillainMinions.HasRows)
                    {
                        int counter = 1;
                        Console.WriteLine($"Villain: {villainName}");
                        while (readerForVillainMinions.Read())
                        {
                            string minionName = (string)readerForVillainMinions["Name"];
                            int minionAge = (int)readerForVillainMinions["Age"];
                            Console.WriteLine($"{counter}. {minionName} {minionAge}");
                        }
                    }
                }
            }
        }
    }
}
