using Spectre.Console;
using System.Drawing;
using System.Xml.Linq;

namespace HabitLogger.Entities
{
    public static class HabitUI
    {
        public async static Task AddHabitUI()
        {
            var habitName = AnsiConsole.Ask<string>("What is the [green]habit[/]?");
            var habitQuantity = AnsiConsole.Ask<int>("[green]Repetitions[/]?");
            var habitTime = AnsiConsole.Ask<DateTime>("date of [green]occurrence[/]?");

            var table = new Table();

            // Add columns
            table.AddColumn("Name");
            table.AddColumn("Quantity");
            table.AddColumn("Time");

            // Add rows
            table.AddRow(habitName, habitQuantity.ToString(), habitTime.ToString("dd/MM/yyyy"));

            AnsiConsole.Write(table);

            var confirmed = AnsiConsole.Confirm("Place this order?");

            if (confirmed)
            {
                AnsiConsole.MarkupLine("[green]Order placed![/]");
                await DataAccess.AddHabit(habitName, habitQuantity, habitTime);
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Order cancelled.[/]");
            }
        }

        public static int ShowMenu()
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]Welcome, the HabitLogger[/]!")
                    .AddChoices("Add", "Remove", "Get", "Exit"));

            if (selection == "Add")
                return 1;
            if (selection == "Remove")
                return 2;
            if (selection == "Get")
                return 3;
            else
                return 0;
        }
    }
}
