using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UML.GUI
{
    public class ArrowFactory
    {
        static public PathGeometry CreateAggregate(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            double length = CalculateLength(startPoint, endPoint);
            double angle = CalculateAngle(startPoint, endPoint, length);

            PathGeometry arrow = new PathGeometry();
            PathFigureCollection figureCollection = new PathFigureCollection();
            PathSegmentCollection segHeadCollection = new PathSegmentCollection();
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X - 7, endPoint.Y - length + 15), true));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y - length + 30), true));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X + 7, endPoint.Y - length + 15), true));
            PathFigure arrowHead = new PathFigure(new System.Windows.Point(endPoint.X, endPoint.Y - length), segHeadCollection, true);

            PathSegmentCollection segTailCollection = new PathSegmentCollection();
            segTailCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y - length + 30), true));
            PathFigure arrowTail = new PathFigure(new System.Windows.Point(endPoint.X, endPoint.Y ), segTailCollection, false);
            figureCollection.Add(arrowTail);
            figureCollection.Add(arrowHead);
            arrow.Figures = figureCollection;
            arrow.Transform = new RotateTransform(angle, endPoint.X, endPoint.Y);

            return arrow;
        }

        static public PathGeometry CreateComposite(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            double length = CalculateLength(startPoint, endPoint);
            double angle = CalculateAngle(startPoint, endPoint, length);

            PathGeometry arrow = new PathGeometry();
            PathFigureCollection figureCollection = new PathFigureCollection();
            PathSegmentCollection segHeadCollection = new PathSegmentCollection();
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X - 7, endPoint.Y - length + 15), true));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y - length + 30), true));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X + 7, endPoint.Y - length + 15), true));
            PathFigure arrowHead = new PathFigure(new System.Windows.Point(endPoint.X, endPoint.Y - length), segHeadCollection, true);

            PathSegmentCollection segTailCollection = new PathSegmentCollection();
            segTailCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y - length + 30), true));
            PathFigure arrowTail = new PathFigure(new System.Windows.Point(endPoint.X, endPoint.Y), segTailCollection, false);
            figureCollection.Add(arrowTail);
            figureCollection.Add(arrowHead);
            arrow.Figures = figureCollection;
            arrow.Transform = new RotateTransform(angle, endPoint.X, endPoint.Y);

            return arrow;
        }

        static public PathGeometry CreateDependency( System.Windows.Point startPoint, System.Windows.Point endPoint )
        {
            double length = CalculateLength(startPoint, endPoint);
            double angle = CalculateAngle(startPoint, endPoint, length);

            PathGeometry arrow = new PathGeometry();
            PathFigureCollection figureCollection = new PathFigureCollection();
            PathSegmentCollection segHeadCollection = new PathSegmentCollection();
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X - 7, endPoint.Y - 15), true));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y), false));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X + 7, endPoint.Y - 15), true));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y), false));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y - length), true));
            PathFigure arrowHead = new PathFigure(endPoint, segHeadCollection, false);
            figureCollection.Add(arrowHead);
            arrow.Figures = figureCollection;
            arrow.Transform = new RotateTransform(angle, endPoint.X, endPoint.Y);

            return arrow;
        }

        static public PathGeometry CreateInheritance( System.Windows.Point startPoint, System.Windows.Point endPoint )
        {
            double length = CalculateLength(startPoint, endPoint);
            double angle = CalculateAngle(startPoint, endPoint, length);

            PathGeometry arrow = new PathGeometry();
            PathFigureCollection figureCollection = new PathFigureCollection();
            PathSegmentCollection segHeadCollection = new PathSegmentCollection();
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X - 7, endPoint.Y - 15), true));
            segHeadCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X + 7, endPoint.Y - 15), true));
            PathFigure arrowHead = new PathFigure(endPoint, segHeadCollection, true);

            PathSegmentCollection segTailCollection = new PathSegmentCollection();
            segTailCollection.Add(new LineSegment(new System.Windows.Point(endPoint.X, endPoint.Y - length), true));
            PathFigure arrowTail = new PathFigure(new System.Windows.Point(endPoint.X, endPoint.Y - 15), segTailCollection, false);
            figureCollection.Add(arrowTail);
            figureCollection.Add(arrowHead);
            arrow.Figures = figureCollection;
            arrow.Transform = new RotateTransform(angle, endPoint.X, endPoint.Y);

            return arrow;
        }

        static double CalculateAngle( System.Windows.Point startPoint, System.Windows.Point endPoint, double length )
        {
            // using dot product with normal pointing down
            double angle = Math.Acos((endPoint.Y - startPoint.Y) / length) * 180/ Math.PI;
            if (endPoint.X > startPoint.X)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        static  double CalculateLength( System.Windows.Point startPoint, System.Windows.Point endPoint )
        {
            double length = Math.Sqrt(Math.Pow((endPoint.X - startPoint.X), 2) + Math.Pow((endPoint.Y - startPoint.Y), 2));

            return length;
        }

    }
}
