using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    public class CommandManager
    {
        // MEMBERS
        private Stack<UndoableCommand> m_undos = new Stack<UndoableCommand>();
        private Stack<UndoableCommand> m_redos = new Stack<UndoableCommand>();

        // METHODS
        public void Execute(Command newCommand)
        {
            newCommand.Execute();
            if( newCommand is UndoableCommand )
            {
                m_undos.Push((UndoableCommand)newCommand);
            }
            m_redos.Clear();
        }

        public void Redo()
        {
            if( m_redos.Count > 0 )
            {
                // Pop off the begin transaction
                UndoableCommand command = m_redos.Pop();
                m_undos.Push(command);

                while ( m_redos.Count > 0 )
                {
                    if( m_redos.Peek() is BeginTransactionCommand )
                    {
                        break;
                    }

                    command = m_redos.Pop();
                    command.Execute();
                    m_undos.Push(command);
                }
            }
        }

        public void Undo()
        {
            if (m_undos.Count > 0)
            {
                while (m_undos.Count > 0)
                {
                    UndoableCommand command = m_undos.Pop();
                    command.Undo();

                    m_redos.Push(command);

                    if( command is BeginTransactionCommand )
                    {
                        break;
                    }
                }
            }
        }

        public bool CanUndo
        {
            get { return m_undos.Count > 0; }
        }

        public bool CanRedo
        {
            get { return m_redos.Count > 0; }
        }
    }
}
