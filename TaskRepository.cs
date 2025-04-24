using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    /// <summary>
    /// Класс обеспечивает методы для работы с хранилищем задач через LINQ-запросы.
    /// </summary>
    class TaskRepository
    {
        private static string _filePath;

        public static void SetFilePath(string path)
        {
            _filePath = path;
        }

        public static List<Task> ReadTasks()
        {
            FileStream fs = null;
            BinaryReader reader = null;
            List<Task> tasks = new List<Task>();

            try
            {
                fs = new FileStream(_filePath, FileMode.Open);
                reader = new BinaryReader(fs, Encoding.UTF8);

                while (fs.Position < fs.Length)
                {
                    var id = reader.ReadInt32();
                    var title = reader.ReadString();
                    var description = reader.ReadString();
                    var dueDate = DateTime.FromBinary(reader.ReadInt64());
                    var isCompleted = reader.ReadBoolean();
                    var task = new Task(id, title, description, dueDate, isCompleted);
                    tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении задач: {ex.Message}");
            }
            if (reader != null)
            {
                reader.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }
            return tasks;
        }

        public static void WriteTasks(List<Task> tasks)
        {
            FileStream fs = null;
            BinaryWriter writer = null;

            try
            {
                fs = new FileStream(_filePath, FileMode.Create);
                writer = new BinaryWriter(fs, Encoding.UTF8);

                foreach (var task in tasks)
                {
                    writer.Write(task.Id);
                    writer.Write(task.Title);
                    writer.Write(task.Description);
                    writer.Write(task.DateCompletion.ToBinary());
                    writer.Write(task.Status);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи задач: {ex.Message}");
            }
            if (writer != null)
            {
                writer.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }
        }

        public static void AddTask(Task task)
        {
            List<Task> tasks = ReadTasks();
            int newId;

            if (tasks.Count > 0)
            {
                int maxId = tasks.Max(t => t.Id);
                newId = maxId + 1;
            }
            else
            {
                newId = 1;
            }
            task.Id = newId;

            tasks.Add(task);
            WriteTasks(tasks);
        }

        public static bool DeleteTask(int id)
        {
            List<Task> tasks = ReadTasks();
            var taskToRemove = tasks.FirstOrDefault(t => t.Id == id);

            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
                WriteTasks(tasks);
                return true;
            }
            return false;
        }

        public static List<Task> GetOverdueTasks()
        {
            List<Task> tasks = ReadTasks();
            List<Task> overdueTasks = tasks.Where(t => t.DateCompletion < DateTime.Now && !t.Status).ToList();
            return overdueTasks;
        }

        public static List<Task> GetCompletedTasks()
        {
            List<Task> tasks = ReadTasks();
            List<Task> completedTasks = tasks.Where(t => t.Status).ToList();
            return completedTasks;
        }

        /// <summary>
        /// помечает задачу как завершенную по указанному ID
        /// </summary>
        /// <param name="id"> идентификатор задачи, которую необходимо пометить как завершенную </param>
        public static void MarkTaskAsCompleted(int id)
        {
            List<Task> tasks = ReadTasks();
            var taskToUpdate = tasks.FirstOrDefault(t => t.Id == id);

            if (taskToUpdate != null)
            {
                taskToUpdate.Status = true;
                WriteTasks(tasks);
            }
            else
            {
                Console.WriteLine($"\nЗадача с ID {id} не найдена.");
            }
        }

        public static Task GetTaskById(int id)
        {
            List<Task> tasks = ReadTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            return task;
        }

        public static List<Task> GetTasksByDate(DateTime date)
        {
            List<Task> tasks = ReadTasks();
            List<Task> taskByDate = tasks.Where(t => t.DateCompletion.Date == date.Date).ToList();
            return taskByDate;
        }

        public static List<Task> GetTasksOrderedByDate(bool ascending)
        {
            List<Task> tasks = ReadTasks();
            List<Task> orderedTasks;

            if (ascending)
            {
                orderedTasks = tasks.OrderBy(t => t.DateCompletion).ToList();
            }
            else
            {
                orderedTasks = tasks.OrderByDescending(t => t.DateCompletion).ToList();
            }
            return orderedTasks;
        }
    }
}
