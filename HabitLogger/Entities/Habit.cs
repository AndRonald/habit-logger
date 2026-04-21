namespace HabitLogger.Entities
{
    public class Habit
    {
        public int Id { get; set; }
        public string? HabitName { get; set; }
        public int Quantity { get; set; }
        public DateTime HabitDate { get; set; }
    }
}
