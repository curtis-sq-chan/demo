using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using UML.ViewModel.Diagram;

namespace UML.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // METHODS

        public MainWindow()
        {
            InitializeComponent();
        }

        private void onDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thumb = (Thumb)sender;

            Node node = (Node)thumb.DataContext;
            node.X += e.HorizontalChange;
            node.Y += e.VerticalChange;
        }

        // TODO: usercontrol??
        private void SetNodeSizeHandler(object sender, RoutedEventArgs e)
        {
            Thumb thumb = (Thumb)sender;
            Node node = (Node)thumb.DataContext;
            node.Height = thumb.ActualHeight;
            node.Width = thumb.ActualWidth;
        }

        // TODO: usercontrol??
        private void SetNodeSizeHandler(object sender, SizeChangedEventArgs e)
        {
            Thumb thumb = (Thumb)sender;
            Node node = (Node)thumb.DataContext;
            node.Height = thumb.ActualHeight;
            node.Width = thumb.ActualWidth;
        }

        private void ClearTagInput(object sender, RoutedEventArgs e)
        {
            TextBox textInput = (TextBox)sender;
            textInput.Clear();
        }

        private void NewNodeDragOver(object sender, DragEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            ClassDiagram diagram = (ClassDiagram)canvas.DataContext;
            Guid classId = (Guid)e.Data.GetData("Integer");
            if( diagram.Nodes.Contains(classId) )
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private void CloseWindowHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
