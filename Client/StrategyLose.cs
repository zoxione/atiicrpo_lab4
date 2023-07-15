using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Класс StrategyLose
    /// </summary>
    public class StrategyLose : Strategy
    {
        /// <summary>
        /// Выполнение стратегии
        /// утери книги
        /// </summary>
        /// <param name="idSubscriber">Номер абонента</param>
        /// <param name="idBook">Номер экземпляра книги</param>
        /// <param name="period">Не используется</param>
        /// <returns>Код результата (0 - успешно, 2 - нет такой книги, 3 - нет абонента)</returns>
        public override int Execute(Guid idSubscriber, Guid idCopyBook, TimeSpan? period)
        {
            // получаем абонента
            Subscriber subscriber = Library.Instance.Subscribers.ToList().Find(x => x.Id == idSubscriber);
            if (subscriber == null)
            {
                return 3;
            }

            // получаем экзепляр книги
            CopyBook copyBook = subscriber.CopyBooks.ToList().Find(x => x.Id == idCopyBook);
            if (copyBook == null)
            {
                return 2;
            }

            // получаем книгу
            Book book = Library.Instance.Books.ToList().Find(x => x.Id == copyBook.IdBook);
            if (book == null)
            {
                return 2;
            }

            // забираем экземпляр у абонента
            subscriber.CopyBooks.Remove(copyBook);

            // уменьшаем количество выданных
            book.CountIssueCopies--;

            // списание книги
            book.CountCopies--;
            subscriber.CountFine += 1000;

            return 0;
        }
    }
}
