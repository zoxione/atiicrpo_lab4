using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class FictionBook : Book
    {
        public FictionBook(Guid id, string title, string author, string description, string department, string location, int countCopies) : base(id, title, author, description, department, location, countCopies)
        { }
    }
}
