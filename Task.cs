namespace Planner
{
    /// <summary>
    /// Представляет задачу с заданными свойствами, такими как ID, заголовок, описание, дата выполнения и статус завершенности.
    /// </summary>
    public class Task
    {
        private uint _id;
        private string _title;
        private string _description;
        private DateTime _dateCompletion;
        private bool _status;

        public Task(uint id, string title, string description, DateTime dateCompletion, bool status)
        {
            _id = id;

            if (string.IsNullOrWhiteSpace(title))
            {
                _title = "Без названия";
            }
            else
            {
                _title = title;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                _description = "Нет описания";
            }
            else
            {
                _description = description;
            }

            if (dateCompletion < DateTime.MinValue || dateCompletion > DateTime.MaxValue)
            {
                _dateCompletion = DateTime.Now;
                Console.WriteLine("Введена недопустимая дата. Установлено значение по умолчанию - текущее время.");
            }
            else
            {
                _dateCompletion = dateCompletion;
            }

            _status = status;
        }

        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DateTime DateCompletion
        {
            get { return _dateCompletion; }
            set { _dateCompletion = value; }
        }

        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public override string ToString()
        {
            return $"\nID: {_id}, Название: {_title}, Описание: {_description}, Дата выполнения: {_dateCompletion.ToShortDateString()}, Выполнение: {_status}";
        }
    }
}
