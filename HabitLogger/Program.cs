using HabitLogger.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HabitLogger
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            bool operation = true;
            await DataAccess.CreateTable();
            while (operation)
            {
                
                int choice = HabitUI.ShowMenu(); 
                switch (choice)
                {
                    case 1:
                        await HabitUI.AddHabitUI();
                        break;
                    //case 2:
                    //    Console.Write("Deseja remover qual hábito?: ");
                    //    int habitId = int.Parse(Console.ReadLine()!);
                    //    RemoveHabit(habitId).Wait();
                    //    break;
                    case 3:
                        await DataAccess.GetHabits();
                        break;
                    //case 4:
                    //    Console.Write("Digite o número do hábito: ");
                    //    int id = int.Parse(Console.ReadLine()!);
                    //    Console.Write("Digite qual nome gostaria de dar: ");
                    //    string habit = Console.ReadLine()!;
                    //    Console.Write("Digite a quantidade de vezes repetidas: ");
                    //    int newQuantity = int.Parse(Console.ReadLine()!);
                    //    await UpdateHabit(id, habit, newQuantity);
                    //    break;
                    case 0:
                        operation = false;
                        break;

                }
            }

        }
    }
}