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
    /// Класс CopyBook
    /// представляет экземпляр книги
    /// </summary>
    public class CopyBook : INotifyPropertyChanged
    {
        private Guid _id;
        private Guid _idBook;
        private Guid _idSubscriber;
        private DateTime _issueDate;
        private TimeSpan _period;

        /// <summary>
        /// Номер экзепляра
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        /// <summary>
        /// Номер книги
        /// </summary>
        public Guid IdBook
        {
            get { return _idBook; }
            set { _idBook = value; OnPropertyChanged("IdBook"); }
        }

        /// <summary>
        /// Номер абонента
        /// </summary>
        public Guid IdSubscriber
        {
            get { return _idSubscriber; }
            set { _idSubscriber = value; OnPropertyChanged("IdSubscriber"); }
        }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public DateTime IssueDate
        {
            get { return _issueDate; }
            set { _issueDate = value; OnPropertyChanged("IssueDate"); }
        }

        /// <summary>
        /// Период
        /// </summary>
        public TimeSpan Period
        {
            get { return _period; }
            set { _period = value; OnPropertyChanged("Period"); }
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public CopyBook(Guid id, Guid idBook, Guid idSubscriber, DateTime issueDate, TimeSpan period) 
        { 
            _id = id;
            _idBook = idBook;
            _idSubscriber = idSubscriber;
            _issueDate = issueDate;
            _period = period;
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
