using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    class FinishMoveCommand : UndoableCommand
    {
        // MEMBERS
        private double m_xOffset;
        private double m_yOffset;
        private Node m_node;
        bool m_isRedo = false;

        // METHODS
        public FinishMoveCommand(Node node, double xOffset, double yOffset)
        {
            this.m_node = node;
            this.m_xOffset = xOffset;
            this.m_yOffset = yOffset;
        }

        public override void Execute()
        {
            if( m_isRedo )
            {
                m_node.X += m_xOffset;
                m_node.Y += m_yOffset;
            }
            m_isRedo = true;
        }

        public override void Undo()
        {
            m_node.X -= m_xOffset;
            m_node.Y -= m_yOffset;
        }
    }
}
