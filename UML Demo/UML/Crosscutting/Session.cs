using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Crosscutting
{
    public class ItemEventArgs : EventArgs
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }
    }

    // conatins session caches and user credentials
    public class Session
    {
        private ObjectCache m_itemCache = null;
        private ViewModel.ModelViewTracker m_modelViewTracker = null;

        // EVENTS
        public event EventHandler<ItemEventArgs> ItemAdded;
        public event EventHandler<ItemEventArgs> ItemUpdated;
        public event EventHandler<ItemEventArgs> ItemRemoved;

        public ObjectCache ItemCache
        {
            get { return m_itemCache; }
            set
            {
                m_itemCache = value;
            }
        }

        public ViewModel.ModelViewTracker ModelViewTracker
        {
            get { return m_modelViewTracker; }
            set
            {
                m_modelViewTracker = value;
            }
        }

        public void OnItemAdded(ItemEventArgs e)
        {
            EventHandler<ItemEventArgs> handler = ItemAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void OnItemUpdated(ItemEventArgs e)
        {
            EventHandler<ItemEventArgs> handler = ItemUpdated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void OnItemRemoved(ItemEventArgs e)
        {
            EventHandler<ItemEventArgs> handler = ItemRemoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
