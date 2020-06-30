
namespace SvgRenderer
{


    public class SvgColor
    {


        public static readonly string Black = "#000";
        public static readonly string White = "#FFF";
        public static readonly string Green = "#00F";


        private static readonly char[] base16 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };


        public static string ToWebRgb(int rgb)
        {
            char[] hex = new char[] {
                    '#',
                    base16[(rgb >> 20) & 15],
                    base16[(rgb >> 16) & 15],
                    base16[(rgb >> 12) & 15],
                    base16[(rgb >> 8) & 15],
                    base16[(rgb >> 4) & 15],
                    base16[rgb & 15],
                };

            string rgbString = new string(hex, 0, hex.Length);
            return rgbString;
        }


        public static string ToWebRgb(byte r, byte g, byte b)
        {
            char[] hex = new char[] {
                    '#',
                    base16[(r >> 4) & 15],
                    base16[r & 15],
                    base16[(g >> 4) & 15],
                    base16[g & 15],
                    base16[(b >> 4) & 15],
                    base16[b & 15],
                };

            string rgb = new string(hex, 0, hex.Length);
            return rgb;
        }


        public static string ToWebArgb(int argb)
        {
            char[] hex = new char[] {
                '#', 
                base16[(argb >> 28) & 15],
                base16[(argb >> 24) & 15],
                base16[(argb >> 20) & 15],
                base16[(argb >> 16) & 15],
                base16[(argb >> 12) & 15],
                base16[(argb >> 8) & 15],
                base16[(argb >> 4) & 15],
                base16[argb & 15],
            };

            string argbString = new string(hex, 0, hex.Length);
            return argbString;
        }

        public static string ToWebArgb(byte a, byte r, byte g, byte b)
        {
            char[] hex = new char[] {
                    '#',
                    base16[(a >> 4) & 15],
                    base16[a & 15],
                    base16[(r >> 4) & 15],
                    base16[r & 15],
                    base16[(g >> 4) & 15],
                    base16[g & 15],
                    base16[(b >> 4) & 15],
                    base16[b & 15],
                };

            string rgb = new string(hex, 0, hex.Length);
            return rgb;
        }


#if SLOW 
        public static string FromArgb(int red, int green, int blue)
        {
            int rgb = (red << 16) + (green << 8) + blue;
            return "#" + rgb.ToString("X6", System.Globalization.CultureInfo.InvariantCulture);
            // return "#" + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
            // return $"rgb({red},{green},{blue})";
        }
#endif 


    }


}
