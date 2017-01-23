using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UML.Repository.Local
{
    public class ItemRepository<T> where T : Domain.CatalogueItem
    {
        // Use typeof for changelist
        // Then plug in ItemRepository<System.Type>
        // TODO: how does the tag repo work now?

        private Crosscutting.Session m_session = null;
        private ItemCacheImpl m_itemCache = null;

        public ItemRepository(Crosscutting.Session session)
        {
            m_session = session;
            m_itemCache = (ItemCacheImpl)m_session.ItemCache;
        }

        public bool Add(T item)
        {
            m_itemCache.Loaded.Add(item);

            Crosscutting.ItemEventArgs addEvent = new Crosscutting.ItemEventArgs();
            addEvent.Id = item.Id;
            addEvent.Name = item.Name;
            addEvent.Type = typeof(T);
            m_session.OnItemAdded(addEvent);

            return true;
        }
        
        public void Remove(T item)
        {
            Crosscutting.ItemEventArgs removeEvent = new Crosscutting.ItemEventArgs();
            removeEvent.Id = item.Id;
            removeEvent.Name = item.Name;
            removeEvent.Type = typeof(T);
            m_session.OnItemRemoved(removeEvent);
        }

        public void Update(T item)
        {
            Crosscutting.ItemEventArgs updateEvent = new Crosscutting.ItemEventArgs();
            updateEvent.Id = item.Id;
            updateEvent.Name = item.Name;
            updateEvent.Type = typeof(T);
            m_session.OnItemUpdated(updateEvent);
        }

        public T GetById(Guid id)
        {
            // look in the cache first
            IEnumerable<Domain.CatalogueItem> matchingItems =
                from currentItem in m_itemCache.Loaded where currentItem.Id == id select currentItem;

            T result = (T)matchingItems.ElementAt(0);

            return result;
        }

        public IEnumerable<ItemMetadata> GetAll()
        {
            List<ItemMetadata> manifest = new List<ItemMetadata>();
            return manifest;
        }
    }
}
