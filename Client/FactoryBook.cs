using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Класс FactoryBook
    /// представляет фабрику объектов
    /// </summary>
    public static class FactoryBook
    {
        public enum IdTypeBook { idFictionBook, idTechnicalBook };

        /// <summary>
        /// Фабричный метод
        /// создания книги
        /// </summary>
        public static Book CreateBook(IdTypeBook idTypeBook, string title, string author, string description, string location, int countCopies)
        {
            switch (idTypeBook)
            {
                case IdTypeBook.idFictionBook:
                    return new FictionBook(Guid.NewGuid(), title, author, description, "художественный", location, countCopies);
                case IdTypeBook.idTechnicalBook:
                    return new TechnicalBook(Guid.NewGuid(), title, author, description, "технический", location, countCopies);
                default:
                    return null;
            }
        }
    }
}
