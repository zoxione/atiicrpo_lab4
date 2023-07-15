using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Абстрактный класс Strategy
    /// </summary>
    public abstract class Strategy
    {
        /// <summary>
        /// Виртуальный метод
        /// выполнения стратегии
        /// </summary>
        public abstract int Execute(Guid idSubscriber, Guid idBook, TimeSpan? period);
    }
}
