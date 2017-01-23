using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UML.ViewModel
{
    public abstract class ModelViewObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_enableReadFromModel = true;

        protected void OnPropertyChanged( string propertyName )
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs( propertyName ));
            }
        }

        protected void Notify(object sender, EventArgs e)
        {
            if( m_enableReadFromModel )
            {
                ReadFromModel();
            }
        }

        public void UpdateModel()
        {
            m_enableReadFromModel = false;
            WriteToModel();
            m_enableReadFromModel = true;
        }

        // Push the changes in this view model and broadcast the view has been updated
        protected abstract void WriteToModel();

        // Update the view reading from the model
        protected abstract void ReadFromModel();
    }
}
