using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Абстрактный класс Book
    /// представляет книгу
    /// </summary>
    public abstract class Book : INotifyPropertyChanged
    {
        protected Guid _id = Guid.NewGuid();
        protected string _title = "";
        protected string _author = "";
        protected string _description = "";
        protected string _department = "";
        protected string _location = "";
        protected int _countCopies = 0;
        protected int _countIssueCopies = 0;

        /// <summary>
        /// Номер книги
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        /// <summary>
        /// Имя книги
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        /// <summary>
        /// Автор книги
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; OnPropertyChanged("Author"); }
        }

        /// <summary>
        /// Описание книги
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        /// <summary>
        /// Отдел книги
        /// </summary>
        public string Department
        {
            get { return _department; }
            set { _department = value; OnPropertyChanged("Department"); }
        }

        /// <summary>
        /// Место нахождения
        /// </summary>
        public string Location
        {
            get { return _location; }
            set { _location = value; OnPropertyChanged("Location"); }
        }

        /// <summary>
        /// Количество экземпляров всего
        /// </summary>
        public int CountCopies
        {
            get { return _countCopies; }
            set { _countCopies = value; OnPropertyChanged("CountCopies"); }
        }

        /// <summary>
        /// Количество экземпляров выданных
        /// </summary>
        public int CountIssueCopies
        {
            get { return _countIssueCopies; }
            set { _countIssueCopies = value; OnPropertyChanged("CountIssueCopies"); }
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Book(Guid id, string title, string author, string description, string department, string location, int countCopies)
        {
            _id = id;
            _title = title;
            _author = author;
            _description = description;
            _department = department;
            _location = location;
            _countCopies = countCopies;
            _countIssueCopies = 0;
        }

        // MVVM
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
