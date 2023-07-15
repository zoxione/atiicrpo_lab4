using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Класс StrategyIssue
    /// </summary>
    public class StrategyIssue : Strategy
    {
        /// <summary>
        /// Выполнение стратегии
        /// выдачи книги
        /// </summary>
        /// <param name="idSubscriber">Номер абонента</param>
        /// <param name="idBook">Номер книги</param>
        /// <param name="period">На какой период</param>
        /// <returns>Код результата (0 - успешно, 1 - книги закончились, 2 - нет такой книги, 3 - нет абонента)</returns>
        public override int Execute(Guid idSubscriber, Guid idBook, TimeSpan? period)
        {
            // получаем книгу
            Book book = Library.Instance.Books.ToList().Find(x => x.Id == idBook);
            if (book == null)
            {
                return 2;
            }

            // проверка на количество
            if ((book.CountCopies - book.CountIssueCopies) <= 0)
            {
                return 1;
            }

            // получаем абонента
            Subscriber subscriber = Library.Instance.Subscribers.ToList().Find(x => x.Id == idSubscriber);
            if (subscriber == null)
            {
                return 3;
            }

            // создаем экзепляр книги
            CopyBook copyBook = new CopyBook(Guid.NewGuid(), idBook, subscriber.Id, DateTime.Now, period.Value);

            // даем абоненту
            subscriber.CopyBooks.Add(copyBook);

            // увеличиваем количество выданных
            book.CountIssueCopies++;

            return 0;
        }
    }
}
