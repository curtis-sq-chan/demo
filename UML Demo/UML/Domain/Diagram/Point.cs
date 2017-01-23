using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Domain.Diagram
{
    [Serializable]
    class Point
    {
        // MEMBERS
        private double m_x;
        private double m_y;

        // METHODS
        public double X
        {
            get { return m_x; }
            set
            {
                m_x = value;
            }
        }

        public double Y
        {
            get { return m_y; }
            set
            {
                m_y = value;
            }
        }
    }
}
