
using System.Drawing;


namespace SvgRenderer
{


    public class SvgPath
    {

        public System.Text.StringBuilder sb;


        public void Reset()
        {
            this.sb = new System.Text.StringBuilder();
            sb.AppendLine("Reset");
        }


        public void CloseFigure()
        {
            sb.AppendLine("CloseFigure");
        }


        //     Adds a cubic Bézier curve to the current figure.
        //   pt1: A System.Drawing.PointF that represents the starting point of the curve.
        //   pt2: A System.Drawing.PointF that represents the first control point for the curve.
        //   pt3: A System.Drawing.PointF that represents the second control point for the curve.
        //   pt4: A System.Drawing.PointF that represents the endpoint of the curve.
        public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            sb.Append("AddBezier ");
            sb.AppendFormat("pt1: ({0}, {1}) ", pt1.X, pt1.X);
            sb.AppendFormat("pt2: ({0}, {1}) ", pt2.X, pt2.X);
            sb.AppendFormat("pt3: ({0}, {1}) ", pt3.X, pt3.X);
            sb.AppendFormat("pt4: ({0}, {1}) ", pt4.X, pt4.X);
            sb.AppendLine("");
        }


        // Appends a line segment to this System.Drawing.Drawing2D.GraphicsPath.
        //   pt1: A System.Drawing.PointF that represents the starting point of the line.
        //   pt2: A System.Drawing.PointF that represents the endpoint of the line.
        public void AddLine(PointF pt1, PointF pt2)
        {
            sb.AppendFormat("AddLine pt1: ({0}, {1}), pt2: ({2}{3}) ", pt1.X, pt1.X, pt2.X, pt2.Y);
            sb.AppendLine("");
        }


    }


}
