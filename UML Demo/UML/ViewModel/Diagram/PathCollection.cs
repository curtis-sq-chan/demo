using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace UML.ViewModel.Diagram
{
    class PathCollection : ObservableCollection<Path>
    {
        public PathCollection()
            : base()
        {
        }
    }
}
