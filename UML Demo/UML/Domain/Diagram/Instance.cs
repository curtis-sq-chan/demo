using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Domain.Diagram
{
    [Serializable]
    public class Instance
    {
        // MEMBERS 
        private string m_alias;
        private Point m_location = new Point();
        private Guid m_class;

        // MEMBERS

        public double X
        {
            get { return m_location.X; }
            set
            {
                m_location.X = value;
            }
        }

        public double Y
        {
            get { return m_location.Y; }
            set
            {
                m_location.Y = value;
            }
        }

        public string Alias
        {
            get { return m_alias; }
            set
            {
                m_alias = value;
            }
        }

        public Guid ClassId
        {
            get { return m_class; }
            set
            {
                m_class = value;
            }
        }

    }
}
