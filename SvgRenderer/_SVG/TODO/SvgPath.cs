﻿
namespace SvgRenderer
{


    public class SvgPath
    {

        public System.Text.StringBuilder sb;


        public void Reset()
        {
            if (this.sb != null)
                this.sb.Length = 0;
            else
                this.sb = new System.Text.StringBuilder();
        } // End Sub Reset 
        
        
        protected void AppendToPath(string textToAppend)
        {
            if (this.sb.Length != 0)
                this.sb.Append(" ");
            
            this.sb.Append(textToAppend);
        } // End Sub AppendToPath 


        public void CloseFigure()
        {
            if (this.sb.Length != 0)
                this.AppendToPath("Z");
        } // End Sub CloseFigure 


        // Adds a cubic Bézier curve to the current figure.
        //   pt1: A System.Drawing.PointF that represents the starting point of the curve.
        //   pt2: A System.Drawing.PointF that represents the first control point for the curve.
        //   pt3: A System.Drawing.PointF that represents the second control point for the curve.
        //   pt4: A System.Drawing.PointF that represents the endpoint of the curve.
        public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            // lowercase c: relative coordinates
            // uppercase C: absolute coordinates
            this.AppendToPath($"M {pt1.X},{pt1.Y} C {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt1.Y}");
            // this.AppendToPath($"M {pt1.X},{pt1.Y} c {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt1.Y}");
        } // End Sub AddBezier 


        // Appends a line segment to this System.Drawing.Drawing2D.GraphicsPath.
        //   pt1: A System.Drawing.PointF that represents the starting point of the line.
        //   pt2: A System.Drawing.PointF that represents the endpoint of the line.
        public void AddLine(PointF pt1, PointF pt2)
        {
            this.AppendToPath($"M{pt1.X},{pt1.Y} L{pt2.X},{pt2.Y}");
        } // End Sub AddLine 


    } // End Class SvgPath 


} // End Namespace SvgRenderer 
