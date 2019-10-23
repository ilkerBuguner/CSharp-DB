using System;
using System.Data.SqlClient;

namespace _06_Remove_Villain
{
    class Program
    {
        private const string ConnectionString = "Server=.\\SQLEXPRESS;" +
                "Database=MinionsDB;" +
                "Integrated Security=true";

        private const string SelectNameOfMinionById = @"SELECT Name FROM Villains 
                                                        WHERE Id = @villainId";

        private const string DeleteMinionsOfVillain = @"DELETE FROM MinionsVillains 
                                                       WHERE VillainId = @villainId";

        private const string DeleteVillain = @"DELETE FROM Villains
                                               WHERE Id = @villainId";

        private static SqlTransaction transaction;
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                transaction = connection.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = SelectNameOfMinionById;
                    command.Parameters.AddWithValue("@villainId", id);

                    object value = command.ExecuteScalar();

                    if (value == null)
                    {
                        throw new ArgumentException("No such villain was found.");
                    }

                    string villainName = (string)value;

                    command.CommandText = DeleteMinionsOfVillain;

                    int minionsReleased = command.ExecuteNonQuery();

                    command.CommandText = DeleteVillain;

                    command.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{minionsReleased} minions were released.");
                }
                catch (ArgumentException ae)
                {
                    try
                    {
                        Console.WriteLine(ae.Message);
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                    catch (Exception re)
                    {
                        Console.WriteLine(re.Message);

                    }
                }
                
            }
        }
    }
}
