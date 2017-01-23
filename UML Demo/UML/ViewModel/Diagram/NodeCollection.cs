using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    public class NodeCollection : ObservableCollection<Node>
    {
        public NodeCollection()
            : base()
        {
        }

        public bool Contains(Guid classId )
        {
            bool found = false;

            foreach( Node node in this )
            {
                if( node.Payload.Id == classId )
                {
                    found = true;
                    break;
                }
            }

            return found;
        }
    }
}
