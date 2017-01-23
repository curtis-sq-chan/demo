using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    public abstract class UndoableCommand : Command
    {
        public abstract void Execute();

        public abstract void Undo();
    }
}
