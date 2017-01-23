using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using UML.ViewModel.Diagram;

namespace UML.GUI
{
    class LinkToPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double startX = (double)values[0];
            double startY = (double)values[1];
            double startWidth = (double)values[2];
            double startHeight = (double)values[3];
            double endX = (double)values[4];
            double endY = (double)values[5];
            double endWidth = (double)values[6];
            double endHeight = (double)values[7];
            Link.LinkType linkType = (Link.LinkType)values[8];

            // calculate anchor points for each node
            List<System.Windows.Point> startAnchorPoints = CalculateAnchorPoints(startX, startY, startWidth, startHeight);
            List<System.Windows.Point> endAnchorPoints = CalculateAnchorPoints(endX, endY, endWidth, endHeight);

            // find the shortest path from 16 point combination
            System.Windows.Point startPoint = new System.Windows.Point();
            System.Windows.Point endPoint = new System.Windows.Point();
            double distance = double.MaxValue;
            foreach (System.Windows.Point currentStartPoint in startAnchorPoints)
            {
                foreach (System.Windows.Point currentEndPoint in endAnchorPoints)
                {
                    double currentDistance = Math.Pow((currentEndPoint.X - currentStartPoint.X), 2) + Math.Pow((currentEndPoint.Y - currentStartPoint.Y), 2);
                    if (distance > currentDistance)
                    {
                        startPoint = currentStartPoint;
                        endPoint = currentEndPoint;
                        distance = currentDistance;
                    }
                }
            }

            PathGeometry arrow = null;
            if (linkType == Link.LinkType.Aggregate)
            {
                arrow = ArrowFactory.CreateAggregate(startPoint, endPoint);
            }
            else if (linkType == Link.LinkType.Composite)
            {
                arrow = ArrowFactory.CreateComposite(startPoint, endPoint);
            }
            else if (linkType == Link.LinkType.Dependency)
            {
                arrow = ArrowFactory.CreateDependency(startPoint, endPoint);
            }
            else if (linkType == Link.LinkType.Inheritance)
            {
                arrow = ArrowFactory.CreateInheritance(startPoint, endPoint);
            }
            else
            {
                throw new Exception("Invalid argument.");
            }

            return arrow;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private List<System.Windows.Point> CalculateAnchorPoints( double x, double y, double width, double height )
        {
            List<System.Windows.Point> anchorPoints = new List<System.Windows.Point>();
            double anchorX;
            double anchorY;

            // Top
            anchorX = x + width / 2;
            anchorY = y;
            anchorPoints.Add(new System.Windows.Point(anchorX, anchorY));

            // Left
            anchorX = x;
            anchorY = y + height/2;
            anchorPoints.Add(new System.Windows.Point(anchorX, anchorY));

            // Right
            anchorX = x + width;
            anchorY = y + height / 2;
            anchorPoints.Add(new System.Windows.Point(anchorX, anchorY));

            // Bottom
            anchorX = x + width / 2;
            anchorY = y + height;
            anchorPoints.Add(new System.Windows.Point(anchorX, anchorY));

            return anchorPoints;
        }
    }
}
