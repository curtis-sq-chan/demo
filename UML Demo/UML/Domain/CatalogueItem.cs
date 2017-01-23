using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Domain
{
    public interface CatalogueItem
    {
        Guid Id { set; get; }

        string Name { set; get; }
    }
}
