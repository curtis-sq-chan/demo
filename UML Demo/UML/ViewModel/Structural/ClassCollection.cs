using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel.Structural
{
    public class ClassCollection : ObservableCollection<Class>
    {
        public ClassCollection()
            : base()
        {
            CollectionChanged += OnClassesChanged;
        }

        void OnClassesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (Class currentClass in e.NewItems)
                {
                    currentClass.RequestRemove += OnRequestRemove;
                }
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (Class currentClass in e.OldItems)
                {
                    currentClass.RequestRemove -= OnRequestRemove;
                }
            }
        }

        void OnRequestRemove(object sender, EventArgs e)
        {
            Class currentClass = (Class)sender;
            Remove(currentClass);
        }
    }
}
