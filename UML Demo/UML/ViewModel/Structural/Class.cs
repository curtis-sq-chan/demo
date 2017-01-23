using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UML.ViewModel.Structural
{
    // TODO: need factory, the constructor is too complicated

    public class Class : ModelViewObject
    {
        // MEMBERS
        private string m_name;
        private Domain.Structural.Class m_class;
        private Crosscutting.Session m_session;
        private ClassCollection m_parentClasses = new ClassCollection();
        private ICommand m_saveCommand;

        // EVENTS
        public event EventHandler RequestRemove;

        // EVENTS
        public event EventHandler Saved;

        // COMMANDS
        private bool m_canSave;
        private ICommand m_removeCommand;

        // METHODS
        public Class( Crosscutting.Session session, Domain.Structural.Class classObj)
        {
            m_class = classObj;
            m_session = session;

            m_session.ModelViewTracker.Add(m_class.Id, Notify);
            m_removeCommand = new RelayCommand(OnRemove);
            m_saveCommand = new RelayCommand(OnSave, IsAbleToSave);

            // Load data from model
            ReadFromModel();

            CanSave = false;
        }

        ~Class()
        {
            m_session.ModelViewTracker.Remove(m_class.Id, Notify);
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
                CanSave = true;
            }
        }

        public Guid Id
        {
            get { return m_class.Id; }
        }

        protected override void WriteToModel()
        {
            m_class.Name = m_name;

            m_class.Parents.Clear();
            foreach(Class parentClass in m_parentClasses)
            {
                m_class.Parents.Add(parentClass.Id);
            }

            m_session.ModelViewTracker.NotifyModelChange(m_class.Id);

            Repository.Local.ItemRepository<Domain.Structural.Class> classRepository = Repository.Local.RepositoryFactory.NewClassRepository(m_session);
            classRepository.Update(m_class);
        }

        protected override void ReadFromModel()
        {
            Name = m_class.Name;
        }

        private bool IsAbleToSave(object obj)
        {
            return m_canSave;
        }

        void OnSave(object parameter)
        {
            UpdateModel();
            CanSave = false;
            OnSave(EventArgs.Empty);
        }

        void OnSave(EventArgs e)
        {
            EventHandler handler = Saved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public ClassCollection ParentClasses
        {
            get { return m_parentClasses; }
        }

        public bool CanSave
        {
            get { return m_canSave; }
            set
            {
                m_canSave = value;
                OnPropertyChanged("CanSave");
            }
        }

        public bool IsValidParent( Class parentClass )
        {
            // check if the parent is already added
            foreach (Class parent in m_parentClasses)
            {
               if( parent.Id == parentClass.Id )
               {
                   return false;
               }
            }

            bool isValid = true;

            // check for circular inheritance if the parent is added
            HashSet<Guid> discoveredNodes = new HashSet<Guid>();
            Queue<Class> undiscoveredNodes = new Queue<Class>();
            undiscoveredNodes.Enqueue(parentClass);
            while( undiscoveredNodes.Count > 0 )
            {
                Class currentNode = undiscoveredNodes.Dequeue();
                if( discoveredNodes.Contains( currentNode.Id ) )
                {
                    continue;
                }

                if( currentNode.Id == this.Id )
                {
                    isValid = false;
                    break;
                }

                discoveredNodes.Add(currentNode.Id);

                // add the parents to the queue.
                foreach( Class parent in currentNode.m_parentClasses )
                {
                    undiscoveredNodes.Enqueue(parent);
                }
            }

            return isValid;
        }

        private void OnModified(object sender, EventArgs e)
        {
            CanSave = true;
        }

        void OnRequestRemove(EventArgs e)
        {
            EventHandler handler = RequestRemove;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void OnRemove(object o)
        {
            OnRequestRemove(EventArgs.Empty);
        }

        public ICommand Remove
        {
            get { return m_removeCommand; }
        }

        public ICommand Save
        {
            get { return m_saveCommand; }
        }
    }
}
