using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Repository.Local
{
    public class RepositoryFactory
    {
        private RepositoryFactory()
        {

        }

        public static ItemRepository<Domain.Structural.Class> NewClassRepository(Crosscutting.Session session)
        {
            return new ItemRepository<Domain.Structural.Class>(session);
        }

        public static ItemRepository<Domain.Diagram.ClassDiagram> NewClassDiagramRepository(Crosscutting.Session session)
        {
            return new ItemRepository<Domain.Diagram.ClassDiagram>(session);
        }
    }
}
