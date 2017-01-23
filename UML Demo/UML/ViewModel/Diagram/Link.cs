using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UML.ViewModel.Diagram
{
    public class NodeEventArgs : EventArgs
    {
        private Node m_node = null;

        public Node Node
        {
            get { return m_node; }
            set { m_node = value; }
        }
    }

    public class Link : ModelViewObject
    {
        public enum LinkType
        {
            Aggregate,
            Composite,
            Dependency,
            Inheritance
        };

        // MEMBERS
        private Node m_startPoint = null;
        private Node m_endPoint = null;
        private LinkType m_type;
        private bool m_valid;
        private List<Node> m_nodes = new List<Node>();

        // EVENTS
        public event EventHandler<NodeEventArgs> RequestClose;

        // COMMANDS
        private ICommand m_closeCommand;

        // METHODS
        public Link( Node start, Node end, LinkType type, bool valid )
        {
            m_nodes.Add(start);
            m_nodes.Add(end);
            m_startPoint = start;
            m_endPoint = end;
            m_type = type;
            m_valid = valid;

            m_startPoint.CloseCompleted += OnNodeRemoved;
            m_endPoint.CloseCompleted += OnNodeRemoved;

            m_closeCommand = new RelayCommand(OnClose);
        }

        private void OnNodeRemoved(object sender, EventArgs e)
        {
            Node node = (Node)sender;
            NodeEventArgs args = new NodeEventArgs();
            args.Node = node;
            OnRequestClose(args);
        }

        public Node Start
        {
            get { return m_startPoint; }
        }

        public Node End
        {
            get { return m_endPoint; }
        }

        public bool Valid
        {
            get { return m_valid; }
        }

        public List<Node> AssociatedNodes
        {
            get { return m_nodes; }
        }

        public LinkType Type
        {
            get { return m_type; }
        }

        protected override void WriteToModel()
        {
            
        }

        protected override void ReadFromModel()
        {

        }

        public ICommand Close
        {
            get { return m_closeCommand; }
        }

        private void OnClose(object o)
        {
            NodeEventArgs args = new NodeEventArgs();
            OnRequestClose(args);
        }

        private void OnRequestClose( NodeEventArgs e )
        {
            EventHandler<NodeEventArgs> handler = RequestClose;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
