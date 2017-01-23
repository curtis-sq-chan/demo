using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Domain.Diagram
{
    [Serializable]
    public class Relationship
    {
        public enum RelationshipType
        {
            Aggregate,
            Composite,
            Dependency,
            Inheritance
        };

        private Guid m_startClassId;
        private Guid m_endClassId;
        private RelationshipType m_type;

        // METHODS
        public Relationship()
        {
        }

        public Guid Start
        {
            get { return m_startClassId; }
            set { m_startClassId = value; }
        }

        public Guid End
        {
            get { return m_endClassId; }
            set { m_endClassId = value; }
        }

        public RelationshipType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }
    }
}
