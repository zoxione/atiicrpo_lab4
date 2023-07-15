using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Client
{
    /// <summary>
    /// Класс Library
    /// представляет фасад
    /// </summary>
    public class Library : INotifyPropertyChanged
    {
        private static Library _instance = new Library();
        private ObservableCollection<Book> _books = new ObservableCollection<Book>();
        private Librarian _librarian;
        private ObservableCollection<Subscriber> _subscribers = new ObservableCollection<Subscriber>();
        private ObservableCollection<string> _messages = new ObservableCollection<string>();

        /// <summary>
        /// Экземпляр объекта
        /// </summary>
        public static Library Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Спикок книг
        /// </summary>
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { _books = value; OnPropertyChanged("Books"); }
        }

        /// <summary>
        /// Библиотекарь
        /// </summary>
        public Librarian Librarian
        {
            get { return _librarian; }
            set { _librarian = value; OnPropertyChanged("Librarian"); }
        }

        /// <summary>
        /// Абоненеты
        /// </summary>
        public ObservableCollection<Subscriber> Subscribers
        {
            get { return _subscribers; }
            set { _subscribers = value; OnPropertyChanged("Subscribers"); }
        }

        /// <summary>
        /// Список сообщений
        /// </summary>
        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged("Messages"); }
        }

        /// <summary>
        /// Констуктор по умолчанию
        /// </summary>
        private Library() { }

        /// <summary>
        /// Выполнение стратегии
        /// </summary>
        public int Execute(Strategy strategy, Guid idSubscriber, Guid idBook, TimeSpan? period)
        {
            return strategy.Execute(idSubscriber, idBook, period);
        }

        /// <summary>
        /// Очищение библиотеки
        /// </summary>
        public void Clear()
        {
            _subscribers.Clear();
            //_selectedSubscriber
            _books.Clear();
            _messages.Clear();
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
