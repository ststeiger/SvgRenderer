
namespace SvgRenderer
{


    class ProgrammaticallyInstallFontWindows
    {

        public static void foo()
        {
            // So the best option i found is to copy the font to windows font directory
            System.IO.File.Copy("MyNewFont.ttf", System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows), "Fonts", "MyNewFont.ttf"));
            // And then add respective entries in registery,Like

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
            key.SetValue("My Font Description", "fontname.tff");
            key.Close();
            // Also try, pastebin.com/C99TmXBn
        }




        [System.Runtime.InteropServices.DllImport("gdi32", EntryPoint = "AddFontResource")]
        public static extern int AddFontResourceA(string lpFileName);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern int AddFontResource(string lpszFilename);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern int CreateScalableFontResource(uint fdwHidden, string
        lpszFontRes, string lpszFontFile, string lpszCurrentPath);

        /// <summary>
        /// Installs font on the user's system and adds it to the registry so it's available on the next session
        /// Your font must be included in your project with its build path set to 'Content' and its Copy property
        /// set to 'Copy Always'
        /// </summary>
        /// <param name="contentFontName">Your font to be passed as a resource (i.e. "myfont.tff")</param>
        private static void RegisterFont(string contentFontName)
        {
            // Creates the full path where your font will be installed
            string fontDestination = System.IO.Path.Combine(System.Environment.GetFolderPath
                                              (System.Environment.SpecialFolder.Fonts), contentFontName);

            if (!System.IO.File.Exists(fontDestination))
            {
                // Copies font to destination
                System.IO.File.Copy(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), contentFontName), fontDestination);

                // Retrieves font name
                // Makes sure you reference System.Drawing
                System.Drawing.Text.PrivateFontCollection fontCol = new System.Drawing.Text.PrivateFontCollection();
                fontCol.AddFontFile(fontDestination);
                string actualFontName = fontCol.Families[0].Name;

                //Add font
                AddFontResource(fontDestination);
                //Add registry entry  
                Microsoft.Win32.Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts",
                actualFontName, contentFontName, Microsoft.Win32.RegistryValueKind.String);
            }

        }


    }


}
