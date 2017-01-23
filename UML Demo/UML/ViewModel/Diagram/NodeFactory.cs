using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    class NodeFactory
    {
        public Node CreateClassNode(Crosscutting.Session session, Structural.Class newClass)
        {
            // TODO: instance might need to be linked to the class id
            Domain.Diagram.Instance newInstance = new Domain.Diagram.Instance();
            Node newNode = new Node(newInstance, newClass);

            return newNode;
        }

        
        public Node CreateClassNode(Crosscutting.Session session, Guid classId )
        {
            Structural.ClassFactory classFactory = new Structural.ClassFactory();
            Structural.Class newClass = classFactory.CreateClass(session, classId);

            // TODO: instance might need to be linked to the class id
            Domain.Diagram.Instance newInstance = new Domain.Diagram.Instance();
            Node newNode = new Node(newInstance, newClass);

            return newNode;
        }
    }
}
