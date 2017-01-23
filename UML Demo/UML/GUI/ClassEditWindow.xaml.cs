using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UML.GUI
{
    /// <summary>
    /// Interaction logic for ClassEditWindow.xaml
    /// </summary>
    public partial class ClassEditWindow : Window
    {
        public ClassEditWindow()
        {
            InitializeComponent();
        }

        private void CloseWindowHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void ClassNameFocus(object sender, RoutedEventArgs e)
        {
            TextBox input = (TextBox)sender;
            if (input.Text == "NewNode")
            {
                input.Text = "";
            }
        }
    }
}
