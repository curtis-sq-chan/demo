using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Domain.Diagram
{
    [Serializable]
    public class ClassDiagram : CatalogueItem
    {
        // MEMBERS
        private Guid m_id;
        private string m_name;
        private List<Instance> m_instances = new List<Instance>();
        private List<Relationship> m_relationships = new List<Relationship>();

        // METHODS
        public ClassDiagram()
        {
            m_name = "Untitled";
        }

        public List<Instance> Instances
        {
            get { return m_instances; }
            set { m_instances = value; }
        }

        public List<Relationship> Relationships
        {
            get { return m_relationships; }
            set { m_relationships = value; }
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
            }
        }

        public Guid Id
        {
            get { return m_id; }
            set { m_id = value; }
        }
    }
}
