using Spectre.Console;

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

            table.AddColumn("Name");
            table.AddColumn("Quantity");
            table.AddColumn("Time");

            table.AddRow(habitName, habitQuantity.ToString(), habitTime.ToString("dd/MM/yyyy"));

            AnsiConsole.Write(table);

            var confirmed = AnsiConsole.Confirm("Confirms the inclusion of this habit?");

            if (confirmed)
            {
                AnsiConsole.MarkupLine("[green]Inclusion successfully![/]");
                await DataAccess.AddHabit(habitName, habitQuantity, habitTime);
            }
            else
                AnsiConsole.MarkupLine("[yellow]Inclusion cancelled.[/]");
            
            Thread.Sleep(2000);
            AnsiConsole.Clear();
        }

        public async static Task RemoveHabitUI()
        {
            await GetAllHabitsUI();

            var habitId = AnsiConsole.Ask<int>("Which one you want to [green]delete[/]? Id:");

            var confirmed = AnsiConsole.Confirm("Confirms the removed of this habit?");

            if (confirmed)
            {
                var habitById = await DataAccess.GetHabitById(habitId);
                await DataAccess.RemoveHabit(habitById);
                AnsiConsole.MarkupLine("[yellow]habit removed![/]");
            }
            else
                AnsiConsole.MarkupLine("[yellow]remove cancelled![/]");

            Thread.Sleep(2000);
            AnsiConsole.Clear();
        }

        public async static Task UpdateUI()
        {
            await GetAllHabitsUI();

            var habit = AnsiConsole.Ask<int>("Which one you want to [green]delete[/]? Id:");
            var habitName = AnsiConsole.Ask<string>("What is the [green]habit[/]?");
            var habitQuantity = AnsiConsole.Ask<int>("[green]Repetitions[/]?");
            var habitTime = AnsiConsole.Ask<DateTime>("date of [green]occurrence[/]?");


            var confirmed = AnsiConsole.Confirm("Confirms the removed of this habit?");

            if (confirmed)
            {
                var habitById = await DataAccess.GetHabitById(habit);
                await DataAccess.UpdateHabit(habit, habitName, habitQuantity, habitTime);
                AnsiConsole.MarkupLine("[yellow]habit updated![/]");
            }
            else
                AnsiConsole.MarkupLine("[yellow]cancel update![/]");


        }

        public async static Task GetAllHabitsUI()
        {
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Quantity");
            table.AddColumn("Time");

            var habits = await DataAccess.GetHabits();

            foreach (var habit in habits)
            {
                table.AddRow(habit.Id.ToString(),habit.HabitName!,habit.Quantity.ToString(),habit.HabitDate.ToString("dd/MM/yyyy"));
            }
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

        public static int ShowMenu()
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]Welcome, the HabitLogger[/]!")
                    .AddChoices("Add", "Remove", "Get", "Update", "Exit"));

            if (selection == "Add")
                return 1;
            if (selection == "Remove")
                return 2;
            if (selection == "Get")
                return 3;
            if (selection == "Update")
                return 4;
            else
                return 0;
        }
    }
}
