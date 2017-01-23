using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Repository.Local
{
    class ItemCacheImpl : Crosscutting.ObjectCache
    {
        private List<Domain.CatalogueItem> m_loadedItems = new List<Domain.CatalogueItem>();
        private IEnumerable<SearchResult> m_manifest = null;

        // METHODS
        public IEnumerable<SearchResult> Manifest
        {
            get { return m_manifest; }
            set
            {
                m_manifest = value;
            }
        }

        public List<Domain.CatalogueItem> Loaded
        {
            get { return m_loadedItems; }
        }
    }
}
