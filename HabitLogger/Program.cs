using HabitLogger.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;

namespace HabitLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {

            bool operation = true;
            while (operation)
            {
                Menu();
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Write("Qual hábito deseja adicionar?: ");
                        string habitName = Console.ReadLine();
                        Console.Write("Quantidade de vezes que repetiu?: ");
                        int quantity = int.Parse(Console.ReadLine());
                        AddHabit(habitName, quantity);
                        break;

                    case 2:
                        Console.Write("Deseja remover qual hábito?: ");
                        int habitId = int.Parse(Console.ReadLine());
                        RemoveHabit(habitId).Wait();
                        break;
                    case 3:
                        GetHabits().Wait();
                        break;
                    case 4:
                        Console.Write("Digite o número do hábito: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Digite qual nome gostaria de dar: ");
                        string habit = Console.ReadLine();
                        Console.Write("Digite a quantidade de vezes repetidas: ");
                        int newQuantity = int.Parse(Console.ReadLine());
                        UpdateHabit(id, habit, newQuantity);
                        break;
                    case 0:
                        operation = false;
                        break;

                }
            }

        }

        static void Menu()
        {
            Console.WriteLine("Habit Logger: ");
            Console.WriteLine("1 - Adicionar hábito");
            Console.WriteLine("2 - Remover hábito");
            Console.WriteLine("3 - Ver progresso\n");
            Console.WriteLine("4 - Atualizar hábito\n");
            Console.WriteLine("0 - Sair");
        }


        public async static Task AddHabit(string name, int quantity)
        {
            await using (SqlConnection db = new SqlConnection(DataAccess.GetConnectionString()))
            {
                await db.OpenAsync();

                string query = "INSERT INTO habits (habitname, quantity) values(@habitname, @quantity)";
                try
                {
                    await using (SqlCommand cmd = new SqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@habitname", name);
                        cmd.Parameters.AddWithValue("@quantity", quantity);

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
            await using (SqlConnection db = new SqlConnection(DataAccess.GetConnectionString()))
            {
                await db.OpenAsync();

                string query = "SELECT * FROM habits";

                await using (SqlCommand cmd = new SqlCommand(query, db))
                {
                    await using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine($"{reader["idhabit"]} - {reader["habitname"]} - {reader["quantity"]} ");
                        }
                    }
                }
            }
        }

        public async static Task RemoveHabit(int id)
        {
            await using (SqlConnection db = new SqlConnection(DataAccess.GetConnectionString()))
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

        public async static Task UpdateHabit(int id, string testep, int quantity)
        {
            await using (SqlConnection db = new SqlConnection(DataAccess.GetConnectionString()))
            {
                await db.OpenAsync();

                string query = "UPDATE habits SET habitname = CONVERT(VARCHAR(30), @testep), quantity = @quantity"
                    + " where idhabit = @id";

                try
                {
                    await using (SqlCommand cmd = new SqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@idhabit", id);
                        cmd.Parameters.AddWithValue("@habitname", SqlDbType.VarChar);
                        cmd.Parameters["@habitname"].Value = testep;
                        cmd.Parameters.AddWithValue("@quantity", quantity);

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