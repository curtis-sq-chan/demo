using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Repository
{
    class CacheFactory
    {
        private CacheFactory()
        {
        }

        public static Crosscutting.ObjectCache NewItemCache()
        {
            return new Local.ItemCacheImpl();
        }
    }
}
