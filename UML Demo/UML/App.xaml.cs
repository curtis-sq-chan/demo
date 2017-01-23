using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UML
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Tie the windows together
            Crosscutting.Session session = new Crosscutting.Session();
            session.ModelViewTracker = new ViewModel.ModelViewTracker();
            session.ItemCache = Repository.CacheFactory.NewItemCache();

            GUI.MainWindow mainWindow = new GUI.MainWindow();
            mainWindow.DataContext = new ViewModel.RepositoryWorkspace( session, OpenClassWindow);
            //Re-enable normal shutdown mode.
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void OpenClassWindow(object dataContext)
        {
            GUI.ClassEditWindow editWindow = new GUI.ClassEditWindow();
            editWindow.DataContext = dataContext;
            editWindow.ShowDialog();
        }
    }
}
