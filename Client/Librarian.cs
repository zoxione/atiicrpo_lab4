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
    /// Класс Librarian
    /// представляет библиотекаря
    /// </summary>
    public class Librarian : User
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Librarian(Guid id, string name) : base(id, name)
        { }
    }
}
