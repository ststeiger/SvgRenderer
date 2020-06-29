
namespace SvgRenderer
{


    public class PointF
    {

        protected float x;
        protected float y;


        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }


        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }


        public PointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


    }


}

