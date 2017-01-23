using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel
{
    public class ModelViewTracker
    {
        private Dictionary<Guid, EventHandler> m_events = null;

        public ModelViewTracker()
        {
            m_events = new Dictionary<Guid, EventHandler>();
        }

        public void Add(Guid id, EventHandler action )
        {
            if( m_events.ContainsKey(id) )
            {
                m_events[id] += action;
            }
            else
            {
                m_events[id] = action;
            }

        }

        public void Remove(Guid id, EventHandler action )
        {
            if (!m_events.ContainsKey(id))
            {
                return;
            }

            m_events[id] -= action;
            if(m_events[id] == null)
            {
                m_events.Remove(id);
            }
        }

        public void NotifyModelChange(Guid id )
        {
            if( !m_events.ContainsKey(id) )
            {
                return;
            }

            EventHandler handler = m_events[id];
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
