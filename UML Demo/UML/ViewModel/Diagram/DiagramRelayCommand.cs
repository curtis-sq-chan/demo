using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    public class DiagramRelayCommand<T> : UndoableCommand
    {
        private T m_parameter;
        private Action<T> m_execution = null;
        private Action<T> m_undo = null;

        public DiagramRelayCommand(T parameter, Action<T> execution, Action<T> undo)
        {
            m_parameter = parameter;
            m_execution = execution;
            m_undo = undo;
        }

        public override void Execute()
        {
            m_execution(m_parameter);
        }

        public override void Undo()
        {
            m_undo(m_parameter);
        }
    }
}
