
namespace SvgRenderer
{
    
    // Specifies whether smoothing (antialiasing) is applied to lines and curves and
    // the edges of filled areas.
    public enum SmoothingMode
    {
        //
        // Zusammenfassung:
        //     Specifies an invalid mode.
        Invalid = -1,
        //
        // Zusammenfassung:
        //     Specifies no antialiasing.
        Default = 0,
        //
        // Zusammenfassung:
        //     Specifies no antialiasing.
        HighSpeed = 1,
        //
        // Zusammenfassung:
        //     Specifies antialiased rendering.
        HighQuality = 2,
        //
        // Zusammenfassung:
        //     Specifies no antialiasing.
        None = 3,
        //
        // Zusammenfassung:
        //     Specifies antialiased rendering.
        AntiAlias = 4
    }

}
