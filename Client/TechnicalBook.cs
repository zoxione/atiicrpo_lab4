using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TechnicalBook : Book
    {
        public TechnicalBook(Guid id, string title, string author, string description, string department, string location, int countCopies) : base(id, title, author, description, department, location, countCopies)
        { }
    }
}
