using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace UML.ViewModel
{
    class RepositoryWorkspace : ModelViewObject
    {
        private Diagram.DiagramCollection m_diagramCollection;
        private Crosscutting.Session m_session = null;
        private Diagram.ClassDiagram m_activeDiagram;
        private string m_workSpaceName = "UML Demo";
        private Action<object> m_openClass = null;

        // Commands
        private ICommand m_addCommand = null;
        private ICommand m_openClassCommand = null;
        private ICommand m_saveCommand = null;

        public RepositoryWorkspace(
            Crosscutting.Session session,
            Action<object> openClass)
        {
            m_session = session;

            m_diagramCollection = new Diagram.DiagramCollection();
            m_diagramCollection.CollectionChanged += OnDiagramsChanged;

            m_openClass = openClass;
            m_addCommand = new RelayCommand(AddDiagram, IsAbleToAddDiagram);
            m_openClassCommand = new RelayCommand(OnOpenClass);
        }

        private bool AbleToSave(object obj)
        {
            return m_diagramCollection.Count > 0;
        }

        private void OnSave(object obj)
        {
            foreach( Diagram.ClassDiagram diagram in m_diagramCollection )
            {
                ICommand saveCommand = diagram.Save;
                if( saveCommand.CanExecute(null) )
                {
                    saveCommand.Execute(null);
                }
            }
        }

        private bool IsNeutralMode(object o)
        {
            if( m_activeDiagram == null )
            {
                return true;
            }

            return m_activeDiagram.CurrentMode == Diagram.ClassDiagram.Mode.Neutral;
        }

        private bool IsAbleToAddDiagram(object o)
        {
            return IsNeutralMode(null);
        }

        private void CloseAllDiagram(object sender, EventArgs e)
        {
            m_diagramCollection.Clear();
            ActiveDiagram = null;
        }

        private void OnDiagramsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if( e.NewItems != null && e.NewItems.Count > 0 )
            {
                foreach(Diagram.ClassDiagram diagram in e.NewItems)
                {
                    diagram.RequestClose += OnDiagramRequestClose;
                }
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (Diagram.ClassDiagram diagram in e.OldItems)
                {
                    diagram.RequestClose -= OnDiagramRequestClose;
                }
            }
        }

        private void OnDiagramRequestClose( object sender, EventArgs e )
        {
            Diagram.ClassDiagram diagram = (Diagram.ClassDiagram)sender;
            int index = m_diagramCollection.IndexOf(diagram);
            if (index == 0)
            {
                ++index;
            }
            else
            {
                --index;
            }

            if( m_diagramCollection.Count > 1 )
            {
                ActiveDiagram = m_diagramCollection[index];
            }
            else
            {
                ActiveDiagram = null;
            }

            m_diagramCollection.Remove(diagram);
        }

        public Diagram.DiagramCollection Diagrams
        {
            get { return m_diagramCollection; }
        }

        public ICommand Add
        {
            get { return m_addCommand; }
        }

        public ICommand OpenClass
        {
            get { return m_openClassCommand; }
        }

        private void AddDiagram( object parameter )
        {
            ViewModel.Diagram.ClassDiagramFactory classDiagramFactory = new ViewModel.Diagram.ClassDiagramFactory();
            ViewModel.Diagram.ClassDiagram diagram = classDiagramFactory.CreateClassDiagram(m_session);
            m_diagramCollection.Add(diagram);

            ActiveDiagram = diagram;
        }

        public Diagram.ClassDiagram ActiveDiagram
        {
            get { return m_activeDiagram; }
            set
            { 
                m_activeDiagram = value;
                OnPropertyChanged("ActiveDiagram");
            }
        }

        public string WorkSpaceName
        {
            get { return m_workSpaceName; }
            set
            {
                m_workSpaceName = value;
                OnPropertyChanged("WorkSpaceName");
            }
        }

        private void OnOpenClass(object parameter)
        {
            // create the class object and set the context
            Guid id = (Guid)parameter;
            OnOpenClass(id);
        }

        private void OnOpenClass(Guid id)
        {
            Structural.ClassFactory factory = new ViewModel.Structural.ClassFactory();
            Structural.Class classVM = factory.CreateClass(m_session, id);
            m_openClass(classVM);
        }

        private void SetWorKSpaceName(object sender, EventArgs e)
        {
            WorkSpaceName = "UML Demo";
        }

        public ICommand Save
        {
            get { return m_saveCommand; }
        }

        protected override void WriteToModel()
        {

        }

        protected override void ReadFromModel()
        {

        }
    }
}
