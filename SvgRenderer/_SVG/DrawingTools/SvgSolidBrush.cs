
namespace SvgRenderer
{
    public class SvgSolidBrush
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
        
        
        public SvgSolidBrush(string color)
        {
            this.m_col = color;
        }
        
        
        public SvgSolidBrush()
            :this(SvgColor.Black)
        { }
        
        
    }
}