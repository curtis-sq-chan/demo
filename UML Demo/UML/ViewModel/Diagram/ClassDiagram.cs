using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UML.ViewModel.Diagram
{
    public class CreateNodeArgs
    {
        private System.Windows.Point m_position;
        private Guid m_classId = Guid.Empty;

        public System.Windows.Point Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public Guid ClassId
        {
            get { return m_classId; }
            set { m_classId = value; }
        }
    }

    public class ClassDiagram : ModelViewObject
    {
        public enum Mode
        {
            AddNode,
            SelectSource,
            AddLink,
            Neutral
        };

        // MEMBERS
        private string m_name;
        private NodeCollection m_nodes = null;
        private LinkCollection m_links = null;
        private Domain.Diagram.ClassDiagram m_diagram;

        private Mode m_currentMode;
        private Link.LinkType m_currentLinkType;
        private Node m_sourceNode;
        private Crosscutting.Session m_session;
        CommandManager m_commandManager = new CommandManager();
        private Link m_previewLink = null;
        private bool m_canSave;

        // EVENTS
        public event EventHandler RequestClose;

        // COMMANDS
        private ICommand m_closeCommand;
        private ICommand m_undoCommand;
        private ICommand m_redoCommand;
        private ICommand m_addNodeCommand;
        private ICommand m_resetModeComand;
        private List<NamedCommand> m_modeChangeCommands = new List<NamedCommand>();
        private ICommand m_selectSourceCommand;
        private ICommand m_addLinkCommand;
        private ICommand m_previewLinkCommand;
        private ICommand m_removePreviewLinkCommand;
        private ICommand m_saveCommand;

        // METHODS
        public ClassDiagram( Crosscutting.Session session, Domain.Diagram.ClassDiagram diagramObj, NodeCollection nodes, LinkCollection links )
        {
            m_session = session;
            m_name = diagramObj.Name;
            m_diagram = diagramObj;
            m_nodes = nodes;
            m_links = links;
            CurrentMode = Mode.Neutral;
            m_sourceNode = null;

            foreach( Node node in m_nodes )
            {
                node.RequestClose += OnNodeRequestClose;
                node.MoveCompleted += OnMoveCompleted;
            }

            foreach( Link link in m_links )
            {
                link.RequestClose += OnLinkRequestClose;
            }

            m_nodes.CollectionChanged += OnNodesChanged;
            m_links.CollectionChanged += OnLinksChanged;

            m_modeChangeCommands.Add( new NamedCommand("Add Class", new RelayCommand(ChangeAddNodeMode, IsAbleToAddClass)));
            m_modeChangeCommands.Add(new NamedCommand("Add Aggregate", new AddLinkModeCommand(Link.LinkType.Aggregate, ChangeLinkMode, IsNeutralMode)));
            m_modeChangeCommands.Add(new NamedCommand("Add Composite", new AddLinkModeCommand(Link.LinkType.Composite, ChangeLinkMode, IsNeutralMode)));
            m_modeChangeCommands.Add( new NamedCommand("Add Dependency", new AddLinkModeCommand(Link.LinkType.Dependency, ChangeLinkMode, IsNeutralMode)));
            m_modeChangeCommands.Add( new NamedCommand("Add Inheritance", new AddLinkModeCommand(Link.LinkType.Inheritance, ChangeLinkMode, IsNeutralMode)));

            m_closeCommand = new RelayCommand(OnClose, IsNeutralMode);
            m_undoCommand = new RelayCommand(OnUndo, IsAbleToUndo);
            m_redoCommand = new RelayCommand(OnRedo, IsAbleToRedo);
            m_addNodeCommand = new RelayCommand(OnAddNode, IsAddNodeMode);
            m_resetModeComand = new RelayCommand(OnResetMode);
            m_selectSourceCommand = new RelayCommand(OnSelectSourceNode, IsSelectSourceMode);
            m_addLinkCommand = new RelayCommand(OnAddLink, IsAddLinkMode);
            m_previewLinkCommand = new RelayCommand(OnShowPreview, IsAddLinkMode);
            m_removePreviewLinkCommand = new RelayCommand(OnResetPreview, IsAddLinkMode);
            m_saveCommand = new RelayCommand(OnSave, IsAbleToSave);

            CanSave = false;
        }

        private bool IsAbleToSave(object obj)
        {
            return m_canSave && IsNeutralMode(obj);
        }

        private bool IsAbleToUndo(object obj)
        {
            return m_commandManager.CanUndo && IsNeutralMode(obj);
        }

        private bool IsAbleToRedo(object obj)
        {
            return m_commandManager.CanRedo && IsNeutralMode(obj);
        }

        private void OnLinksChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (Link link in e.NewItems)
                {
                    link.RequestClose += OnLinkRequestClose;
                }
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (Link link in e.OldItems)
                {
                    link.RequestClose -= OnLinkRequestClose;
                }
            }
        }

        private void OnLinkRequestClose(object sender, NodeEventArgs e)
        {
            Link link = (Link)sender;

            if( e.Node == null )
            {
                m_commandManager.Execute(new BeginTransactionCommand());
            }

            m_commandManager.Execute(new DiagramRelayCommand<Link>(link, OnRemoveLink, OnAddLink));
        }

        private void OnNodesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (Node node in e.NewItems)
                {
                    node.RequestClose += OnNodeRequestClose;
                    node.MoveCompleted += OnMoveCompleted;
                }
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (Node node in e.OldItems)
                {
                    node.RequestClose -= OnNodeRequestClose;
                    node.MoveCompleted -= OnMoveCompleted;
                }
            }
        }

        private void OnMoveCompleted(object sender, MoveEventArgs e)
        {
            Node node = (Node)sender;

            m_commandManager.Execute(new BeginTransactionCommand());
            m_commandManager.Execute(new FinishMoveCommand(node, e.HorizontalChange, e.VerticalChange));

            CanSave = true;
        }

        private void OnNodeRequestClose(object sender, EventArgs e)
        {
            Node node = (Node)sender;
            m_commandManager.Execute(new BeginTransactionCommand());
            m_commandManager.Execute(new DiagramRelayCommand<Node>(node, OnRemoveNode, OnAddNode));
            node.NofityCompletedClose();
        }

        public Guid Id
        {
            get { return m_diagram.Id; }
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

        private void OnAddNode( Node newNode )
        {
            m_nodes.Add( newNode );

            CanSave = true;
        }

        private void OnAddLink(Link newLink)
        {
            if( newLink.Type == Link.LinkType.Inheritance )
            {
                newLink.Start.Payload.ParentClasses.Add( newLink.End.Payload );
                newLink.Start.Payload.UpdateModel();
            }

            m_links.Add(newLink);

            CanSave = true;
        }

        private void OnRemoveNode(Node node)
        {
            m_nodes.Remove( node );

            CanSave = true;
        }

        private void OnRemoveLink(Link link)
        {
            if( link.Type == Link.LinkType.Inheritance )
            {
                link.Start.Payload.ParentClasses.Remove(link.End.Payload);
                link.Start.Payload.UpdateModel();
            }

            m_links.Remove(link);
        }

        // For binding to the UI
        public NodeCollection Nodes
        {
            get { return m_nodes; }
        }

        // For binding to the UI
        public LinkCollection Links
        {
            get { return m_links; }
        }

        protected override void WriteToModel()
        {
            m_diagram.Name = m_name;
            m_diagram.Instances.Clear();
            m_diagram.Relationships.Clear();

            foreach (Node node in m_nodes)
            {
                node.UpdateModel();
                m_diagram.Instances.Add(node.Instance);
            }

            foreach (Link link in m_links)
            {

                // REVISIT: shouldn't the link already have this?
                Domain.Diagram.Relationship relationship = new Domain.Diagram.Relationship();
                relationship.Start = link.Start.Instance.ClassId;
                relationship.End = link.End.Instance.ClassId;
                relationship.Type = LinkTypeConverter.Convert(link.Type);
                m_diagram.Relationships.Add(relationship);
            }

            Repository.Local.ItemRepository<Domain.Diagram.ClassDiagram> classDiagramRepository = Repository.Local.RepositoryFactory.NewClassDiagramRepository(m_session);
            classDiagramRepository.Update(m_diagram);
        }

        protected override void ReadFromModel()
        {
            // TODO: need factory

        }

        private void OnRequestClose( EventArgs e )
        {
            EventHandler handler = RequestClose;
            if (handler != null)
            {
                handler(this,e);
            }
        }

        private void OnClose(object o)
        {
            OnRequestClose(EventArgs.Empty);
        }

        private bool IsNeutralMode( object o )
        {
            return m_currentMode == Mode.Neutral;
        }

        private bool IsAbleToAddClass(object o)
        {
            return IsNeutralMode(null);
        }

        private bool IsAddNodeMode(object o)
        {
            return m_currentMode == Mode.AddNode;
        }

        private bool IsSelectSourceMode(object o)
        {
            return m_currentMode == Mode.SelectSource;
        }

        private bool IsAddLinkMode(object o)
        {
            return m_currentMode == Mode.AddLink;
        }

        private void OnAddNode( object parameter )
        {
            // REVISIT: if neutral mode is not set then extra nodes will be added while
            //          waiting for the repository to save
            CurrentMode = Mode.Neutral;

            CreateNodeArgs args = (CreateNodeArgs)parameter;

            NodeFactory nodeFactory = new NodeFactory();
            Node newNode;
            Structural.ClassFactory classFactory = new Structural.ClassFactory();
            Structural.Class classVM = classFactory.CreateClass(m_session);
            newNode = nodeFactory.CreateClassNode(m_session, classVM);
            newNode.X = args.Position.X;
            newNode.Y = args.Position.Y;
            m_commandManager.Execute(new BeginTransactionCommand());
            m_commandManager.Execute(new DiagramRelayCommand<Node>(newNode, OnAddNode, OnRemoveNode));
        }

        private void OnResetMode( object parameter )
        {
            if( m_sourceNode != null )
            {
                m_sourceNode.IsSelected = false;
                m_sourceNode = null;
            }

            OnResetPreview(null);

            CurrentMode = Mode.Neutral;
        }

        private void OnSelectSourceNode(object parameter)
        {
            m_sourceNode = (Node)parameter;

            m_sourceNode.IsSelected = true;
            CurrentMode = Mode.AddLink;
        }

        private void OnAddLink(object parameter)
        {
            bool isValidLink = m_previewLink.Valid;

            Node sourceNode = m_sourceNode;
            Node endNode = (Node)parameter;

            OnResetMode(null);

            if( isValidLink )
            {
                Link newLink = new Link(sourceNode, endNode, m_currentLinkType, true);
                m_commandManager.Execute(new BeginTransactionCommand());
                m_commandManager.Execute(new DiagramRelayCommand<Link>(newLink, OnAddLink, OnRemoveLink));
            }
        }

        private void OnResetPreview(object parameter)
        {
            if( m_previewLink == null )
            {
                return;
            }

            m_links.Remove(m_previewLink);
            m_previewLink = null;
        }


        private void OnShowPreview(object parameter)
        {
            Node endNode = (Node)parameter;

            bool isValidLink = true;
            if( m_currentLinkType == Link.LinkType.Inheritance )
            {
                isValidLink = m_sourceNode.Payload.IsValidParent( endNode.Payload );
            }

            m_previewLink = new Link(m_sourceNode, endNode, m_currentLinkType, isValidLink);
            m_links.Add(m_previewLink);
        }

        private void OnUndo(object parameter)
        {
            m_commandManager.Undo();
            CanSave = true;
        }

        private void OnRedo(object parameter)
        {
            m_commandManager.Redo();
            CanSave = true;
        }

        private void ChangeLinkMode( Link.LinkType type )
        {
            m_currentLinkType = type;
            CurrentMode = Mode.SelectSource;
        }

        private void ChangeAddNodeMode( object o )
        {
            CurrentMode = Mode.AddNode;
        }

        public ICommand Close
        {
            get { return m_closeCommand; }
        }

        public IEnumerable<NamedCommand> Modes
        {
            get{ return m_modeChangeCommands.AsEnumerable(); }
        }

        public ICommand Undo
        {
            get { return m_undoCommand; }
        }

        public ICommand Redo
        {
            get { return m_redoCommand; }
        }

        public ICommand ResetMode
        {
            get { return m_resetModeComand; }
        }

        public ICommand AddNode
        {
            get { return m_addNodeCommand; }
        }

        public ICommand SelectSource
        {
            get { return m_selectSourceCommand; }
        }

        public ICommand AddLink
        {
            get { return m_addLinkCommand; }
        }

        public ICommand PreviewLink
        {
            get { return m_previewLinkCommand; }
        }

        public ICommand RemovePreviewLink
        {
            get { return m_removePreviewLinkCommand; }
        }

        public Mode CurrentMode
        {
            get { return m_currentMode; }
            private set
            {
                m_currentMode = value;
                OnPropertyChanged("CurrentMode");
            }
        }

        private void OnSave(object obj)
        {
            UpdateModel();
            CanSave = false;
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

        public ICommand Save
        {
            get { return m_saveCommand; }
        }
    }
}
