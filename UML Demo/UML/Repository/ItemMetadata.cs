using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.Repository
{
    [Serializable]
    public class ItemMetadata
    {
        public enum Status
        {
            Add,
            Remove,
            Update,
            Unchanged
        };

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Status Flag { get; set; }

        public Type Type { get; set; }
    }
}
