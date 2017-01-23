using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UML.ViewModel.Diagram
{
    class AddLinkModeCommand : ICommand
    {
        readonly Link.LinkType m_type;
        readonly Action<Link.LinkType> m_execute;
        readonly Predicate<object> m_canExecute;

        public AddLinkModeCommand(Link.LinkType type, Action<Link.LinkType> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            m_type = type;
            m_execute = execute;
            m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute == null ? true : m_canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            m_execute(m_type);
        }
    }
}
