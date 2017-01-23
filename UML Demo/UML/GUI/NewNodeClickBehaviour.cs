using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UML.GUI
{
    public class NewNodeClickBehaviour
    {
        public static DependencyProperty OnMouseLeftClickProperty = DependencyProperty.RegisterAttached(
            "OnMouseLeftClick",
            typeof(ICommand),
            typeof(NewNodeClickBehaviour),
            new UIPropertyMetadata(NewNodeClickBehaviour.OnMouseLeftClick));

        public static void SetOnMouseLeftClick(DependencyObject target, ICommand value)
        {
            target.SetValue(OnMouseLeftClickProperty, value);
        }

        private static void OnMouseLeftClick(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as FrameworkElement;
            if (element == null)
            {
                throw new InvalidOperationException("This behavior can be attached to a FrameworkElement item only.");
            }

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                element.MouseLeftButtonDown += MouseLeftClick;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                element.MouseLeftButtonDown -= MouseLeftClick;
            }
        }

        private static void MouseLeftClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = (ICommand)element.GetValue(OnMouseLeftClickProperty);
            if (!command.CanExecute(null))
            {
                return;
            }

            System.Windows.Point point = Mouse.GetPosition((System.Windows.IInputElement)sender);

            ViewModel.Diagram.CreateNodeArgs args = new ViewModel.Diagram.CreateNodeArgs();
            args.Position = point;

            command.Execute(args);
        }
    }
}
