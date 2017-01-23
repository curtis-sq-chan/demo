using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UML.ViewModel.Diagram
{
    public class MoveEventArgs : EventArgs
    {
        private double m_horizontalChange;
        private double m_verticalChange;

        public double HorizontalChange
        {
            get { return m_horizontalChange; }
            set { m_horizontalChange = value; }
        }

        public double VerticalChange
        {
            get { return m_verticalChange; }
            set { m_verticalChange = value; }
        }
    }

    public class Node : ModelViewObject
    {
        // MEMBERS
        private string m_alias;
        private double m_x;
        private double m_y;
        private double m_width;
        private double m_height;
        private double m_oldX;
        private double m_oldY;
        private bool m_isSelected;

        private Domain.Diagram.Instance m_instance;
        private Structural.Class m_class;

        // EVENTS
        public event EventHandler RequestClose;
        public event EventHandler CloseCompleted;
        public event EventHandler<MoveEventArgs> MoveCompleted;

        // COMMANDS
        private ICommand m_closeCommand;
        private ICommand m_moveCompletedCommand;
        private ICommand m_moveStartedCommand;

        // METHODS

        // TODO: Consider a factory for creation
        public Node( Domain.Diagram.Instance instanceObj, Structural.Class payload )
        {
            this.m_alias = instanceObj.Alias;
            this.m_x = instanceObj.X;
            this.m_y = instanceObj.Y;

            this.m_instance = instanceObj;
            this.m_class = payload;
            this.m_isSelected = false;

            m_closeCommand = new RelayCommand(OnClose);
            m_moveCompletedCommand = new RelayCommand(OnMoveCompleted);
            m_moveStartedCommand = new RelayCommand(OnMoveStarted);
        }

        public string Alias
        {
            get { return m_alias; }
            set
            {
                m_alias = value;
                OnPropertyChanged( "Alias" );
            }
        }

        public double X
        {
            get { return m_x; }
            set
            {
                m_x = value;
                OnPropertyChanged( "X" );
            }
        }

        public double Y
        {
            get { return m_y; }
            set
            {
                m_y = value;
                OnPropertyChanged( "Y" );
            }
        }

        public Structural.Class Payload
        {
            get { return m_class; }
        }

        public Domain.Diagram.Instance Instance
        {
            get { return m_instance; }
        }

        protected override void WriteToModel()
        {
            m_instance.Alias = m_alias;
            m_instance.X = m_x;
            m_instance.Y = m_y;
            m_instance.ClassId = m_class.Id;
        }

        protected override void ReadFromModel()
        {
            m_alias = m_instance.Alias;
            m_x = m_instance.X;
            m_y = m_instance.Y;
        }

        public double Width
        {
            get { return m_width; }
            set
            {
                m_width = value;
                OnPropertyChanged("Width");
            }
        }

        public double Height
        {
            get { return m_height; }
            set
            {
                m_height = value;
                OnPropertyChanged("Height");
            }
        }

        public bool IsSelected
        {
            get { return m_isSelected; }
            set
            {
                m_isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public ICommand Close
        {
            get { return m_closeCommand; }
        }

        public ICommand CompleteMove
        {
            get { return m_moveCompletedCommand; }
        }

        public ICommand StartMove
        {
            get { return m_moveStartedCommand; }
        }

        private void OnClose(object o)
        {
            EventHandler handler = RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        // REVISIT: not sure if this is the right way to go
        public void NofityCompletedClose()
        {
            EventHandler handler = CloseCompleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnMoveCompleted( object o )
        {
            EventHandler<MoveEventArgs> handler = MoveCompleted;
            MoveEventArgs args = new MoveEventArgs();
            args.HorizontalChange = m_x - m_oldX;
            args.VerticalChange = m_y - m_oldY;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnMoveStarted(object o)
        {
            m_oldX = m_x;
            m_oldY = m_y;
        }
    }
}
