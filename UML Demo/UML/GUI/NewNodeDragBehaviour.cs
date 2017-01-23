using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UML.GUI
{
    public class NewNodeDragBehaviour
    {
        public static DependencyProperty OnDragProperty = DependencyProperty.RegisterAttached(
            "OnDrag",
            typeof(Repository.ItemMetadata),
            typeof(NewNodeDragBehaviour),
            new UIPropertyMetadata(NewNodeDragBehaviour.OnDrag));

        public static void SetOnDrag(DependencyObject target, Repository.ItemMetadata value)
        {
            target.SetValue(OnDragProperty, value);
        }

        private static void OnDrag(DependencyObject target, DependencyPropertyChangedEventArgs e)
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

            // REVISIT: not sure if view should know about ViewModel, is it possible to pass just the id
            Repository.ItemMetadata result = (Repository.ItemMetadata)element.GetValue(OnDragProperty);

            DataObject data = new DataObject();
            data.SetData("Integer", result.Id);

            DragDrop.DoDragDrop(element, data, DragDropEffects.Copy);
        }
    }
}
