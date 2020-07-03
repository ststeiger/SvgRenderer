
namespace SvgRenderer.Trash
{


    public class MimeMagicTest
    {


        public static void Test()
        {
            string originalMagicFile = "/usr/share/mime/magic";
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            basePath = System.IO.Path.Combine(basePath, "..", "..", "..");
            basePath = System.IO.Path.GetFullPath(basePath);
            string jsonMagicFile = System.IO.Path.Combine(basePath, "magic.json");

            string fileName =
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Newtonsoft.Json.dll");
            // ConvertMagicFile(originalMagicFile, jsonMagicFile);

            

            // file aapt.exe
            // PE32 executable for MS Windows (console) Intel 80386 32 - bit

            // ILSpy.exe
            // PE32 executable for MS Windows (GUI) Intel 80386 Mono/.Net assembly
            // locate32.exe; PE32 + executable for MS Windows (GUI) Mono /.Net assembly

            GetFileMime(jsonMagicFile, fileName);
            System.Environment.Exit(0);
        } // End Sub Test 


        public static void ConvertMagicFile(string originalMagicFile, string jsonMagicFile)
        {
            MimeMagicSharp.MimeMagicSharp.ConvertFromOriginalToJson(originalMagicFile, jsonMagicFile);
        } // End Sub ConvertMagicFile 


        public static void GetFileMime(string jsonMagicFile, string fileName)
        {
            //  Read and detect
            using (MimeMagicSharp.MimeMagicSharp ms =
                new MimeMagicSharp.MimeMagicSharp(MimeMagicSharp.EMagicFileType.Json, jsonMagicFile))
            {
                try
                {
                    foreach (MimeMagicSharp.MimeTypeGuess mimeTypeGuess in ms.AssumeMimeType(
                        MimeMagicSharp.EMimeTypeBy.Content,
                        fileName))
                    {
                        //  Iterate over results
                        System.Console.Write("Description: ");
                        System.Console.WriteLine(mimeTypeGuess.Description);
                        System.Console.Write("Mime: ");
                        System.Console.WriteLine(mimeTypeGuess.Name);
                    } // Next mimeTypeGuess 
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }

            } // End using ms 

        } // End Sub GetFileMime 


    } // End Class MimeMagicTest 


} // End Namespace SvgRenderer.Trash 
