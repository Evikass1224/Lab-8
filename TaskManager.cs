
namespace Planner
{
    /// <summary>
    /// Класс управляет задачами и предоставляет интерфейс для взаимодействия с пользователем.
    /// </summary>
    class TaskManager
    {
        public void Menu(out bool exitFlag)
        {
            Console.Write("\nВведите путь к файлу для хранения задач (.bin): ");
            string filePath = InputFileName();
            TaskRepository.SetFilePath(filePath);

            exitFlag = false;

            while (true)
            {
                Console.WriteLine("\nМеню: ");
                Console.WriteLine("1) Просмотр задач");
                Console.WriteLine("2) Добавить задачу");
                Console.WriteLine("3) Удалить задачу");
                Console.WriteLine("4) Просмотр просроченных задач");
                Console.WriteLine("5) Просмотр завершённых задач");
                Console.WriteLine("6) Пометить задачу как выполненную");
                Console.WriteLine("7) Просмотр задачи по ID");
                Console.WriteLine("8) Поиск задач по дате");
                Console.WriteLine("9) Просмотр задач, упорядоченных по дате");
                Console.WriteLine("0) Выход");
                Console.Write("\nВыберите действие: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewTasks();
                        break;
                    case "2":
                        AddTask();
                        break;
                    case "3":
                        DeleteTask();
                        break;
                    case "4":
                        ViewOverdueTasks();
                        break;
                    case "5":
                        ViewCompletedTasks();
                        break;
                    case "6":
                        CompleteTask();
                        break;
                    case "7":
                        ViewTaskById();
                        break;
                    case "8":
                        ViewTasksByDate();
                        break;
                    case "9":
                        ViewTasksOrderedByDate();
                        break;
                    case "0":
                        exitFlag = true;
                        return;
                    default:
                        Console.WriteLine("\nОшибка! Попробуйте снова.");
                        break;
                }
            }
        }

        private string InputFileName()
        {
            string fileName = string.Empty;

            while (true)
            {
                fileName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Console.WriteLine("\nОшибка! Путь к файлу не может быть пустым. Попробуйте снова.");
                    continue;
                }

                if (!File.Exists(fileName))
                {
                    Console.WriteLine("\nОшибка! Файл не найден. Попробуйте снова.");
                    continue;
                }

                return fileName;
            }
        }

        private void ViewTasks()
        {
            List<Task> tasks = TaskRepository.ReadTasks();
            if (!tasks.Any())
            {
                Console.WriteLine("Нет задач для отображения.");
                return;
            }
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }

        private void AddTask()
        {
            Console.Write("\nВведите название задачи: ");
            string title = Console.ReadLine();

            Console.Write("Введите описание задачи: ");
            string description = Console.ReadLine();

            DateTime dateCompletion = DateTime.MinValue;
            bool validDate = false; //флаг для проверки корректности ввода даты

            while (!validDate)
            {
                Console.Write("Введите срок выполнения (дд.мм.гггг): ");

                if (DateTime.TryParse(Console.ReadLine(), out dateCompletion))
                {
                    validDate = true;
                }
                else
                {
                    Console.WriteLine("\nНеверный формат даты! Пожалуйста, попробуйте снова.");
                }
            }

            var task = new Task(0, title, description, dateCompletion, false);
            TaskRepository.AddTask(task);
            Console.WriteLine("\nЗадача добавлена.");
        }

        private void DeleteTask()
        {
            int id;
            bool validId = false; //флаг для проверки корректности ввода id

            while (!validId)
            {
                Console.Write("\nВведите ID задачи для удаления: ");
                validId = int.TryParse(Console.ReadLine(), out id);

                if (validId)
                {
                    bool isDeleted = TaskRepository.DeleteTask(id);

                    if (isDeleted)
                    {
                        Console.WriteLine($"\nЗадача с ID {id} удалена.");
                    }
                    else
                    {
                        Console.WriteLine($"\nЗадача с ID {id} не найдена.");
                    }
                }
                else
                {
                    Console.WriteLine("\nНеверный ввод ID! Пожалуйста, попробуйте снова.");
                }
            }
        }

        private void ViewOverdueTasks()
        {
            List<Task> overdueTasks = TaskRepository.GetOverdueTasks();

            if (!overdueTasks.Any())
            {
                Console.WriteLine("\nНет просроченных задач.");
                return;
            }

            Console.WriteLine("\nПросроченные задачи:");
            foreach (var task in overdueTasks)
            {
                Console.WriteLine(task);
            }
        }

        private void ViewCompletedTasks()
        {
            List<Task> completedTasks = TaskRepository.GetCompletedTasks();
            if (!completedTasks.Any())
            {
                Console.WriteLine("\nНет завершённых задач.");
                return;
            }

            foreach (var task in completedTasks)
            {
                Console.WriteLine(task);
            }
        }

        private void CompleteTask()
        {
            int id;
            bool validId = false; //флаг для проверки корректности ввода id

            while (!validId)
            {
                Console.Write("\nВведите ID задачи для пометки как выполненной: ");
                validId = int.TryParse(Console.ReadLine(), out id);

                if (validId)
                {
                    TaskRepository.MarkTaskAsCompleted(id);
                    Console.WriteLine($"\nЗадача с ID {id} помечена как выполненная.");
                }
                else
                {
                    Console.WriteLine("\nНеверный ввод ID! Пожалуйста, попробуйте снова.");
                }
            }
        }

        private void ViewTaskById()
        {
            int id;
            bool validId = false;  //флаг для проверки корректности ввода id

            while (!validId)
            {
                Console.Write("\nВведите ID задачи для просмотра: ");
                validId = int.TryParse(Console.ReadLine(), out id);

                if (validId)
                {
                    Task task = TaskRepository.GetTaskById(id);
                    if (task != null)
                    {
                        Console.WriteLine(task);
                    }
                    else
                    {
                        Console.WriteLine($"\nЗадача с ID {id} не найдена.");
                    }
                }
                else
                {
                    Console.WriteLine("\nНеверный ввод ID! Пожалуйста, попробуйте снова.");
                }
            }
        }

        private void ViewTasksByDate()
        {
            DateTime date;
            bool validDate = false;   //флаг для проверки корректности ввода даты

            while (!validDate)
            {
                Console.Write("\nВведите дату для поиска задач (дд.мм.гггг): ");
                validDate = DateTime.TryParse(Console.ReadLine(), out date);

                if (validDate)
                {
                    List<Task> tasks = TaskRepository.GetTasksByDate(date);
                    if (!tasks.Any())
                    {
                        Console.WriteLine("\nНет задач на указанную дату.");
                    }
                    else
                    {
                        Console.WriteLine($"\nЗадачи на {date.ToShortDateString()}:");
                        foreach (var task in tasks)
                        {
                            Console.WriteLine(task);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nНеверный формат даты! Пожалуйста, попробуйте снова.");
                }
            }
        }

        private void ViewTasksOrderedByDate()
        {
            Console.WriteLine("\nВыберите порядок сортировки:");
            Console.WriteLine("1) По возрастанию (от ранней к поздней)");
            Console.WriteLine("2) По убыванию (от поздней к ранней)");

            string sortChoice = Console.ReadLine();
            List<Task> sortedTasks;

            switch (sortChoice)
            {
                case "1":
                    sortedTasks = TaskRepository.GetTasksOrderedByDate(true);
                    break;
                case "2":
                    sortedTasks = TaskRepository.GetTasksOrderedByDate(false);
                    break;
                default:
                    Console.WriteLine("\nОшибка! Неверный выбор.");
                    return;
            }

            if (!sortedTasks.Any())
            {
                Console.WriteLine("Нет задач для отображения.");
                return;
            }

            Console.WriteLine("\nЗадачи по дате:");
            foreach (var task in sortedTasks)
            {
                Console.WriteLine(task);
            }
        }

    }
}
