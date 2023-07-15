using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Класс Subscriber
    /// представляет абонента
    /// </summary>
    public class Subscriber : User, INotifyPropertyChanged
    {
        private ObservableCollection<CopyBook> _copyBooks = new ObservableCollection<CopyBook>();
        private string _image = "";
        private int _countFine = 0;

        /// <summary>
        /// Книги абонента
        /// </summary>
        public ObservableCollection<CopyBook> CopyBooks
        {
            get { return _copyBooks; }
            set { _copyBooks = value; OnPropertyChanged("CopyBooks"); }
        }

        /// <summary>
        /// Картинка абонента
        /// </summary>
        public string Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("Image"); }
        }

        /// <summary>
        /// Сумма штрафов
        /// </summary>
        public int CountFine
        {
            get { return _countFine; }
            set { _countFine = value; OnPropertyChanged("CountFine"); }
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Subscriber(Guid id, string name, ObservableCollection<CopyBook> copyBooks, string image, int countFine) : base(id, name)
        {
            _copyBooks = copyBooks;
            _image = image;
            _countFine = countFine;
        }
    }
}
