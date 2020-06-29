
namespace SvgRenderer
{
    public class SvgPen
    {
        protected string m_col;

        public string Color 
        {
            get
            {
                return this.m_col;
            }
            set{
                this.m_col = value;
            }
        }
        
        
        public SvgPen(string color)
        {
            this.m_col = color;
        }
        
        
        public SvgPen()
            :this(SvgColor.Black)
        { }
        
        
    }
}