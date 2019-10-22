using System;
using System.Data.SqlClient;
using System.Linq;

namespace _04_Add_Minion
{
    class Program
    {
        private static string connectionString = "Server=DESKTOP-7BEBOBG\\SQLEXPRESS;" +
                "Database=MinionsDB;" +
                "Integrated Security=true";

        static void Main(string[] args)
        {
            string[] minionInfo = Console.ReadLine().Split().Skip(1).ToArray();
            string minionName = minionInfo[0];
            int minionAge = int.Parse(minionInfo[1]);
            string minionTown = minionInfo[2];

            string[] villainInfo = Console.ReadLine().Split().Skip(1).ToArray();
            string villainName = villainInfo[0];

            SqlConnection dbCon = new SqlConnection(connectionString);

            dbCon.Open();

            using (dbCon)
            {
                SqlCommand command = new SqlCommand($" SELECT Id FROM Towns " +
                                                    $" WHERE Name = '{minionTown}'", dbCon);

                object townId = command.ExecuteScalar();
                int existingTownId = -1;

                if (townId == null)
                {
                    command = new SqlCommand($" INSERT INTO Towns (Name) VALUES ('{minionTown}')", dbCon);
                    command.ExecuteNonQuery();

                    Console.WriteLine($"Town {minionTown} was added to the database.");
                }
                command = new SqlCommand($" SELECT Id FROM Towns " +
                                         $" WHERE Name = '{minionTown}'", dbCon);
                existingTownId = (int)command.ExecuteScalar();

                command = new SqlCommand($" SELECT Id FROM Villains WHERE Name = '{villainName}'", dbCon);

                object villainId = command.ExecuteScalar();
                int existingVillainId = -1;

                if (villainId == null)
                {
                    command = new SqlCommand($" INSERT INTO Villains (Name, EvilnessFactorId)  VALUES" +
                                             $" ('{villainName}', 4)", dbCon);
                    command.ExecuteNonQuery();

                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                command = new SqlCommand($" SELECT Id FROM Villains WHERE Name = '{villainName}'", dbCon);
                existingVillainId = (int)command.ExecuteScalar();


                command = new SqlCommand($" INSERT INTO Minions (Name, Age, TownId) VALUES" +
                                         $" ('{minionName}', {minionAge}, {existingTownId})", dbCon);
                command.ExecuteNonQuery();

                command = new SqlCommand($" SELECT Id FROM Minions WHERE Name = '{minionName}'", dbCon);
                int existingMinionId = (int)command.ExecuteScalar();

                command = new SqlCommand($" INSERT INTO MinionsVillains (MinionId, VillainId) VALUES " +
                                         $" ({existingVillainId}, {existingMinionId})", dbCon);

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
        }
    }
}
