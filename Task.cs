
namespace Planner
{
    /// <summary>
    /// Представляет задачу с заданными свойствами, такими как ID, заголовок, описание, дата выполнения и статус завершенности.
    /// </summary>
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCompletion { get; set; }
        public bool Status { get; set; }

        public Task(int id, string title, string description, DateTime dateCompletion, bool status)
        {
            Id = id;
            Title = title;
            Description = description;
            DateCompletion = dateCompletion;
            Status = status;
        }

        public override string ToString()
        {
            return $"\nID: {Id}, Название: {Title}, Описание: {Description}, Дата выполнения: {DateCompletion.ToShortDateString()}, Выполнение: {Status}";
        }
    }
}
