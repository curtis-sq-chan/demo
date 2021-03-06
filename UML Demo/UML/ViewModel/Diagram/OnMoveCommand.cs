﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Diagram
{
    class OnMoveCommand : Command
    {
        // MEMBERS
        private double m_xOffset;
        private double m_yOffset;
        private Node m_node;

        // METHODS
        public OnMoveCommand(Node node, double xOffset, double yOffset)
        {
            this.m_node = node;
            this.m_xOffset = xOffset;
            this.m_yOffset = yOffset;
        }

        public void Execute()
        {
            m_node.X += m_xOffset;
            m_node.Y += m_yOffset;
        }
    }
}
