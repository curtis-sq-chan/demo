using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Domain.Structural
{
    [Serializable]
    public class Class : CatalogueItem
    {
        private Guid m_id;
        private string m_name;

        private List<Guid> m_parentClasses = new List<Guid>();

        public Class()
        {
            m_name = "New Class";
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public Guid Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public List<Guid> Parents
        {
            get { return m_parentClasses; }
            set { m_parentClasses = value; }
        }
    }
}
