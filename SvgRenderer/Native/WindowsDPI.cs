
namespace SvgRenderer.Native
{


    public class DeviceDPI
    {
        /*

https://developer.mozilla.org/en-US/docs/Web/API/Window/devicePixelRatio
https://developer.mozilla.org/en-US/docs/Web/API/Window/matchMedia

devicePixelRatio, deviceXDPI
var scale = window.devicePixelRatio; // Change to 1 on retina screens to see blurry canvas.
var size = 200;
canvas.width = Math.floor(size * scale)+"px";
canvas.height = Math.floor(size * scale)+"px";


function calcScreenDPI() 
{
    const el = document.createElement('div');
    el.style = 'width: 1in;'
    document.body.appendChild(el);
    const dpi = el.offsetWidth; // or .offsetHeight
    document.body.removeChild(el);

    return dpi;
}


let mqString = `(resolution: ${window.devicePixelRatio}dppx)`;

function updatePixelRatio() 
{
  let pr = window.devicePixelRatio;
  let prString = (pr * 100).toFixed(0);
  // pixelRatioBox.innerText = `${prString}% (${pr.toFixed(2)})`;
  console.log(`${prString}% (${pr.toFixed(2)})`);
}

updatePixelRatio();

matchMedia(mqString).addListener(updatePixelRatio);


https://superuser.com/questions/1019825/why-are-pixels-square
https://en.wikipedia.org/wiki/Pixel_aspect_ratio

https://stackoverflow.com/questions/422296/how-do-i-determine-the-true-pixel-size-of-my-monitor-in-net
WPF's True Size = Pixels * DPI Magnification
DPI Magnification:

Matrix dpiMagnification
= PresentationSource.FromVisual(MyUserControl).CompositionTarget.TransformToDevice;
double magnificationX = dpiMagnification.M11;
double magnificationY = dpiMagnification.M22;
        */


        public static readonly int DPI;
        public static readonly int HorizontalDPI;
        public static readonly int VerticalDPI;

        static DeviceDPI()
        {
            // Unknown Platform defaults to 96
            // Note different screens can have different PPIs
            // this is not accounted for here, just uses main display 
            DPI = 96;
            HorizontalDPI = 96;
            VerticalDPI = 96;


            // First filter OSX, that makes it easier
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                // TODO: Implement
                return;
            }

            // Linux or any UNIX
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                DPI = LinuxDPI.DPI;
                HorizontalDPI = LinuxDPI.HorizontalDPI;
                VerticalDPI = LinuxDPI.VerticalDPI;
                return;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                // System.Windows.Froms.Screen.PrimaryScreen.Bounds.Size
                // or Screen.GetBounds(myform))

                DPI = WindowsDPI.DPI;
                HorizontalDPI = DPI;
                VerticalDPI = DPI;
                return;
            }

        } // ENd Static Constructor 

    } // End Class DeviceDPI 



    // https://stackoverflow.com/questions/2621439/how-to-get-screen-dpi-linux-mac-programatically
    class LinuxDPI
    {

        private const string LIBX11 = "libX11";


        // https://linux.die.net/man/3/xclosedisplay
        // Display *XOpenDisplay(char *display_name);
        // in POSIX, if the display_name is NULL, 
        // it defaults to the value of the DISPLAY environment variable.
        [System.Runtime.InteropServices.DllImport(LIBX11, EntryPoint = "XOpenDisplay", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError = true)]
        private static extern System.IntPtr XOpenDisplay(System.IntPtr display);


        // https://linux.die.net/man/3/xclosedisplay
        // int XCloseDisplay(Display *display);
        [System.Runtime.InteropServices.DllImport(LIBX11, EntryPoint = "XCloseDisplay", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError = true)]
        private static extern int XCloseDisplay(System.IntPtr display);



        // https://linux.die.net/man/3/displaywidthmm
        // int DisplayHeight(Display *display, int screen_number);
        [System.Runtime.InteropServices.DllImport("libX11", EntryPoint = "DisplayHeight", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DisplayHeight(System.IntPtr display, int screen_number);

        // https://linux.die.net/man/3/displaywidthmm
        // int DisplayHeightMM(Display *display, int screen_number);
        [System.Runtime.InteropServices.DllImport(LIBX11, EntryPoint = "DisplayHeightMM", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DisplayHeightMM(System.IntPtr display, int screen_number);

        // https://linux.die.net/man/3/displaywidthmm
        // int DisplayWidth(Display *display, int screen_number);
        [System.Runtime.InteropServices.DllImport(LIBX11, EntryPoint = "DisplayWidth", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DisplayWidth(System.IntPtr display, int screen_number);

        // https://linux.die.net/man/3/displaywidthmm
        // int DisplayWidthMM(Display *display, int screen_number);
        [System.Runtime.InteropServices.DllImport(LIBX11, EntryPoint = "DisplayWidthMM", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DisplayWidthMM(System.IntPtr display, int screen_number);


        public static int DPI
        {
            get
            {
                return HorizontalDPI;
            }
        }

        public static int HorizontalDPI
        {
            get
            {
                int screenNumber = 0;
                int returnValue = 96;
                const double mmPerInch = 25.4;

                System.IntPtr display = XOpenDisplay(System.IntPtr.Zero);
                if (display == System.IntPtr.Zero)
                    return returnValue;


                int width_px = DisplayWidth(display, screenNumber);
                int width_mm = DisplayWidthMM(display, screenNumber);

                if (width_mm != 0)
                {
                    double numInch = width_mm / mmPerInch;
                    returnValue = (int)(width_px / numInch);
                }

                int xclosedisplay = XCloseDisplay(System.IntPtr.Zero);
                return returnValue;
            }
        }


        public static int VerticalDPI
        {
            get
            {
                int screenNumber = 0;
                int returnValue = 96;
                const double mmPerInch = 25.4;

                System.IntPtr display = XOpenDisplay(System.IntPtr.Zero);
                if (display == System.IntPtr.Zero)
                    return returnValue;


                int height_px = DisplayHeight(display, screenNumber);
                int height_mm = DisplayHeightMM(display, screenNumber);

                if (height_mm != 0)
                {
                    double numInch = height_mm / mmPerInch;
                    returnValue = (int)(height_px / numInch);
                }

                int xclosedisplay = XCloseDisplay(System.IntPtr.Zero);
                return returnValue;
            }
        }


    }


    // https://www.davidthielen.info/programming/2007/05/get_screen_dpi_.html
    public class WindowsDPI
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateDC", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern System.IntPtr CreateDC(string lpszDriver, string lpszDeviceName, string lpszOutput, System.IntPtr devMode);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool DeleteDC(System.IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", SetLastError = true)]
        private static extern int GetDeviceCaps(System.IntPtr hdc, int capindex);
        private const int LOGPIXELSX = 88;

        private static int _dpi = -1;
        public static int DPI
        {
            get
            {
                if (_dpi != -1)
                    return _dpi;

                _dpi = 96;
                try
                {
                    System.IntPtr hdc = CreateDC("DISPLAY", null, null, System.IntPtr.Zero);
                    if (hdc != System.IntPtr.Zero)
                    {
                        _dpi = GetDeviceCaps(hdc, LOGPIXELSX);
                        if (_dpi == 0)
                            _dpi = 96;
                        DeleteDC(hdc);
                    }
                }
                catch (System.Exception)
                {
                }

                return _dpi;
            }
        }
    }



}
