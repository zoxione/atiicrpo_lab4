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
    /// Абстрактный класс User
    /// </summary>
    public abstract class User : INotifyPropertyChanged
    {
        protected Guid _id = Guid.NewGuid();
        protected string _name = "";

        /// <summary>
        /// Номер абонента
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        /// <summary>
        /// Имя абонента
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public User(Guid id, string name)
        {
            _id = id;
            _name = name;
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
