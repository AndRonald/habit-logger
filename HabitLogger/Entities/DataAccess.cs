using Microsoft.Data.SqlClient;

namespace HabitLogger.Entities
{
    public static class DataAccess
    {
        private readonly static string _connectionString = "Server=(localdb)\\RonaldProjetos;Database=Habits;Trusted_Connection=True;";
        public static string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
