using Microsoft.Data.SqlClient;

namespace HabitLogger.Entities
{
    public static class DataAccess
    {
        private readonly static string _connectionString = "Server=(localdb)\\RonaldProjetos;Database=Habits;Trusted_Connection=True;";
        
        public async static Task CreateTable()
        {
            await using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();

                string query = @"
                    IF OBJECT_ID('dbo.habit', 'U') IS NULL
                    BEGIN
                        CREATE TABLE dbo.habit (
                            IdHabit INT IDENTITY(1,1) PRIMARY KEY,
                            HabitName VARCHAR(100) NOT NULL,
                            Quantity INT NOT NULL,
                            HabitDate DATE NOT NULL
                        )
                    END";
                await using (SqlCommand cmd = new SqlCommand(query, db))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async static Task AddHabit(string habitName, int quantity, DateTime habitDate)
        {
            await using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();

                string query = "INSERT INTO HABIT (HABITNAME, QUANTITY,HABITDATE) values(@habitname, @habitquantity, @habitdate)";
                try
                {
                    await using (SqlCommand cmd = new SqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@habitname", habitName);
                        cmd.Parameters.AddWithValue("@habitquantity", quantity);
                        cmd.Parameters.AddWithValue("@habitdate", habitDate);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao adicionar hábito: " + ex.Message);
                }
            }
        }

        public async static Task GetHabits()
        {
            await using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();

                string query = "SELECT idhabit, habitname, quantity, CONVERT(VARCHAR(30), habitdate, 103) as habitdate FROM habit";

                await using (SqlCommand cmd = new SqlCommand(query, db))
                {
                    await using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine($"{reader["idhabit"]} - {reader["habitname"]} - {reader["quantity"]} - {reader["habitdate"]} ");
                        }
                    }
                }
            }
        }

        public async static Task RemoveHabit(int id)
        {
            await using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();

                string query = "DELETE FROM habits where idhabit = @idhabit";

                try
                {
                    await using (SqlCommand cmd = new SqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@idhabit", id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao remover hábito." + ex.Message);
                }
            }
        }

        public async static Task UpdateHabit(int id, string habitName, int quantity, DateTime habitDate)
        {
            await using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();

                string query = @"
                    UPDATE HABIT 
                    SET 
                        HABITNAME= @habitname, 
                        QUANTITY = @quantity, 
                        HABITDATE = @habitdate 
                    WHERE IDHABIT = @idhabit";

                try
                {
                    await using (SqlCommand cmd = new SqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@habitname", habitName);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@habitdate", habitDate);
                        cmd.Parameters.AddWithValue("@idhabit", id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao atualizar um hábito." + ex.Message);
                }
            }
        }
    }
}
