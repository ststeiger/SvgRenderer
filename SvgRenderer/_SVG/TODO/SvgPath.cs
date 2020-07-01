
namespace SvgRenderer
{
    
    
    public class SvgPath
    {
        
        
        public System.Text.StringBuilder sb;
        protected PointF m_initialPoint;
        
        
        public void Reset()
        {
            if (this.sb != null)
            {
                this.sb.Length = 0;
                this.m_initialPoint = null;
            }
            else
                this.sb = new System.Text.StringBuilder();
        } // End Sub Reset 
        
        
        // https://github.com/mono/libgdiplus/blob/master/src/graphics-path.c#L109
        // static VOID
        // append (GpPath *path, float x, float y, PathPointType type, BOOL compress)
        protected void AppendToPath(string textToAppend)
        {
            if (this.sb.Length != 0)
                this.sb.Append(" ");
            
            this.sb.Append(textToAppend);
        } // End Sub AppendToPath 
        
        
        public void CloseFigure()
        {
            if (this.sb.Length != 0)
            {
                this.AppendToPath("Z");
                // this.AppendToPath($"m 0,0 L{this.m_initialPoint.X},{this.m_initialPoint.Y}");
            }
            
            m_initialPoint = null;
        } // End Sub CloseFigure 
        
        
        // https://github.com/mono/libgdiplus/blob/master/src/graphics-path.c#L892
        // https://github.com/mono/libgdiplus/blob/master/src/graphics-path.c#L157
        // Adds a cubic Bézier curve to the current figure.
        //   pt1: A System.Drawing.PointF that represents the starting point of the curve.
        //   pt2: A System.Drawing.PointF that represents the first control point for the curve.
        //   pt3: A System.Drawing.PointF that represents the second control point for the curve.
        //   pt4: A System.Drawing.PointF that represents the endpoint of the curve.
        public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            if (this.m_initialPoint == null)
            {
                this.m_initialPoint = pt1;
                // this.AppendToPath($"M {pt1.X},{pt1.Y} C {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt1.Y}"); funny
                this.AppendToPath($"M {pt1.X},{pt1.Y} C {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt4.Y}");
            }
            else
            {
                //this.AppendToPath($"C {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt4.Y}");
                // this.AppendToPath($"L {pt1.X},{pt1.Y} C {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt1.Y}"); // funny 
                this.AppendToPath($"L {pt1.X},{pt1.Y} C {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt4.Y}");
            }

            // lowercase c: relative coordinates
            // uppercase C: absolute coordinates

            // this.AppendToPath($"M {pt1.X},{pt1.Y} c {pt2.X},{pt2.Y} {pt3.X},{pt3.Y} {pt4.X},{pt1.Y}");
        } // End Sub AddBezier 
        
        
        // Appends a line segment to this System.Drawing.Drawing2D.GraphicsPath.
        //   pt1: A System.Drawing.PointF that represents the starting point of the line.
        //   pt2: A System.Drawing.PointF that represents the endpoint of the line.
        public void AddLine(PointF pt1, PointF pt2)
        {
            if (this.m_initialPoint == null)
            {
                this.m_initialPoint = pt1;
                this.AppendToPath($"M{pt1.X},{pt1.Y} L{pt2.X},{pt2.Y}");
            }
            else
            {
                // this.AppendToPath($"L{pt2.X},{pt2.Y}");
                this.AppendToPath($"L {pt1.X},{pt1.Y} L{pt2.X},{pt2.Y}");
                // this.AppendToPath($"M{pt1.X},{pt1.Y} L{pt2.X},{pt2.Y}");
            }
            
        } // End Sub AddLine 
        
        
    } // End Class SvgPath 
    
    
} // End Namespace SvgRenderer 
