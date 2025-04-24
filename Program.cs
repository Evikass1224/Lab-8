
namespace Planner
{
    /// <summary>
    /// Класс является точкой входа в приложение для управления задачами.
    /// </summary>
    class Program
    {
        static void Main()
        {
            bool exitFlag;

            TaskManager taskManager = new TaskManager();
            taskManager.Menu(out exitFlag);

            if (exitFlag)
            {
                Console.WriteLine("Выход из программы.");
            }
        }
    }
}