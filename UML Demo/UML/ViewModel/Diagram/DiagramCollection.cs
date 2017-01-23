using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    public class DiagramCollection: ObservableCollection<ClassDiagram>
    {
        public DiagramCollection()
            : base()
        {
        }
    }
}
