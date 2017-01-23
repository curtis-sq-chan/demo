using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Repository
{
    public interface SearchResult
    {
        Guid Id
        {
            get;
        }

        string Title
        {
            get;
        }
    }
}
