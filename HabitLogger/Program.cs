using HabitLogger.Entities;
using Spectre.Console;

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
                        var addPanel = new Panel(
                            new Text(
                                "ADD HABIT", new Style(foreground: Color.Yellow))
                        );
                        AnsiConsole.Write(addPanel);
                        await HabitUI.AddHabitUI();
                        break;
                    case 2:
                        var removePanel = new Panel(
                                new Text("REMOVE HABIT", new Style(foreground: Color.Yellow))
                        );
                        AnsiConsole.Write(removePanel);
                        await HabitUI.RemoveHabitUI();
                        break;
                    case 3:
                        var getAllPanel = new Panel(
                                new Text("GET HABIT", new Style(foreground: Spectre.Console.Color.Yellow))
                        );
                        AnsiConsole.Write(getAllPanel);
                        await HabitUI.GetAllHabitsUI();
                        break;
                    case 4:
                        var updatePanel = new Panel(
                                new Text("UPDATE HABIT", new Style(foreground: Spectre.Console.Color.Yellow))
                        );
                        AnsiConsole.Write(updatePanel);
                        await HabitUI.UpdateUI();
                        break;
                    case 0:
                        operation = false;
                        break;

                }
            }

        }
    }
}