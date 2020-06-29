
namespace SvgRenderer
{
    class SvgGraphics
        : System.IDisposable
    {

        protected System.Text.StringBuilder m_stringBuilder;


        public SvgGraphics()
            : this(new System.Text.StringBuilder())
        { }

        public SvgGraphics(System.Text.StringBuilder sb)
        {
            this.m_stringBuilder = sb;
        }


        // Clears the entire drawing surface and fills it with the specified background color.
        //   color: System.Drawing.Color structure that represents the background color of the drawing surface.
        public void Clear(System.Drawing.Color color)
        {
            this.m_stringBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:svg=""http://www.w3.org/2000/svg"">");
            m_stringBuilder.AppendLine();
            
            // id="svg2" version="1.1"
            // viewBox="0 0 1000 1187.198"
            // width="1000" height="1187.198"
            
            this.m_stringBuilder.AppendLine("<!--");
            this.m_stringBuilder.AppendFormat("Clear: R: {0}, G: {1}, B: {2}", color.R, color.G, color.B);
            this.m_stringBuilder.AppendLine("-->");
            this.m_stringBuilder.AppendLine();
        }

        public void WriteEndFile()
        {
            this.m_stringBuilder.AppendLine("</svg>");
        }

        // Applies the specified scaling operation to the transformation matrix of this
        // System.Drawing.Graphics by prepending it to the object's transformation matrix.
        //   sx: Scale factor in the x direction.
        //   sy: Scale factor in the y direction.
        public void ScaleTransform(float sx, float sy)
        {
            this.m_stringBuilder.AppendLine("<!--");
            this.m_stringBuilder.AppendFormat("ScaleTransform: dx: {0}, dy: {1}", sx, sy);
            this.m_stringBuilder.AppendLine("-->");
            this.m_stringBuilder.AppendLine();
        }


        public SmoothingMode SmoothingMode
        {
            get;set;
        }



        protected string m_translate;


        // Changes the origin of the coordinate system by prepending the specified translation
        // to the transformation matrix of this System.Drawing.Graphics.
        //   dx: The x-coordinate of the translation.
        //   dy: The y-coordinate of the translation.
        public void TranslateTransform(float dx, float dy)
        {
            this.m_translate = "translate(" + dx.ToString(System.Globalization.CultureInfo.InvariantCulture) 
                                            + " " + dy.ToString(System.Globalization.CultureInfo.InvariantCulture)
                                            + ")";
        }

        
        // Fills the interior of a System.Drawing.Drawing2D.GraphicsPath.
        //   brush: System.Drawing.Brush that determines the characteristics of the fill.
        //   path: System.Drawing.Drawing2D.GraphicsPath that represents the path to fill.
        // Ausnahmen: T:System.ArgumentNullException: brush is null. -or- path is null.
        public void FillPath(System.Drawing.Brush brush, SvgPath path)
        {
            this.m_stringBuilder.Append("<path d=\"");
            this.m_stringBuilder.Append(path.sb);
            this.m_stringBuilder.Append("\" style=\"fill: black;stroke:#000;stroke-width:0.26px;\"");

            if (this.m_translate != null)
            {
                this.m_stringBuilder.Append(" transform=\"");
                this.m_stringBuilder.Append(this.m_translate);
                this.m_stringBuilder.Append("\"");
            }
            
            this.m_stringBuilder.AppendLine(" />");
            
            // this.m_stringBuilder.AppendLine("<!--");
            // this.m_stringBuilder.AppendLine("FillPath: ");
            // this.m_stringBuilder.Append(path.sb);
            // this.m_stringBuilder.AppendLine();

            // this.m_stringBuilder.AppendLine("Brush: " + brush.ToString());
            // this.m_stringBuilder.AppendLine("-->");
            // this.m_stringBuilder.AppendLine();
        }


        // Draws a System.Drawing.Drawing2D.GraphicsPath.
        //   pen: System.Drawing.Pen that determines the color, width, and style of the path.
        //   path: System.Drawing.Drawing2D.GraphicsPath to draw.
        // Ausnahmen: T:System.ArgumentNullException: pen is null. -or- path is null.
        public void DrawPath(System.Drawing.Pen pen, SvgPath path)
        {
            this.m_stringBuilder.AppendLine("<!--");
            this.m_stringBuilder.AppendLine("DrawPath: ");
            this.m_stringBuilder.Append(path.sb);
            this.m_stringBuilder.AppendLine();
            this.m_stringBuilder.AppendLine("Pen: " + pen.ToString());
            this.m_stringBuilder.AppendLine("-->");
            this.m_stringBuilder.AppendLine();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }
        
        
    }
    
    
}
