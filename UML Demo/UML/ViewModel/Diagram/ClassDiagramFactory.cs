using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    class ClassDiagramFactory
    {
        public ClassDiagram CreateClassDiagram( Crosscutting.Session session )
        {
            NodeCollection nodeCollection = new NodeCollection();
            LinkCollection linkCollection = new LinkCollection();
            Domain.Diagram.ClassDiagram diagram = new Domain.Diagram.ClassDiagram();
            diagram.Id = Guid.NewGuid();

            ClassDiagram newdiagram = new ClassDiagram(session, diagram, nodeCollection, linkCollection);

            return newdiagram;
        }
    }
}
