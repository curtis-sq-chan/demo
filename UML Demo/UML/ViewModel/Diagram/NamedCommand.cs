using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UML.ViewModel.Diagram
{
    public class NamedCommand
    {
        private string m_displayName;
        private ICommand m_command;

        public NamedCommand( string displayCommand, ICommand command )
        {
            m_displayName = displayCommand;
            m_command = command;
        }

        public string Name
        {
            get { return m_displayName; }
        }

        public ICommand Command
        {
            get { return m_command; }
        }
    }
}
