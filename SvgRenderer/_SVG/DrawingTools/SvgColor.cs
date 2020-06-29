namespace SvgRenderer
{
    public class SvgColor
    {
        public static readonly string Black = "#000";
        public static readonly string White = "#FFF";
        public static readonly string Green = "#00F";

        public static string FromArgb(int red, int green, int blue)
        {
            return $"rgb({red},{green},{blue})";  
        }
        
    }
}