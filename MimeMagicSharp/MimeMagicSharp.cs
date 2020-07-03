
namespace MimeMagicSharp
{
    public enum EMagicFileType { Json, Original }
    public enum EMimeTypeBy { Extension, Content }

    public class MimeMagicSharp 
        : System.IDisposable
    {
        private readonly Reader _mimeReader;
        public static string UnknownMimeType = "application/unknown";

        public static System.Version Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
        
        
        public MimeMagicSharp(EMagicFileType magicFileType, string magicFilePath)
        {
            _mimeReader = new Reader(magicFilePath, magicFileType);
        }
        void System.IDisposable.Dispose()
        {
            _mimeReader?.Dispose();
        }

        public System.Collections.Generic.IEnumerable<MimeTypeGuess> AssumeMimeType(EMimeTypeBy detectionMethod, string filename)
        {
            switch (detectionMethod)
            {
                case EMimeTypeBy.Content:
                    return _mimeReader.GetMimeTypeByContent(filename);
                case EMimeTypeBy.Extension:
                    return _mimeReader.GetMimeTypeByExtension(filename);
                default:
                    throw new System.ArgumentException($"Ivalid property: {detectionMethod}");
            }
        }

        public static void ConvertFromOriginalToJson(string filenameFrom, string filenameTo)
        {
            if (System.IO.File.Exists(filenameFrom))
            {
                Reader mimeReader = new Reader(filenameFrom, EMagicFileType.Original);
                mimeReader.SaveLocal(filenameTo);
            }
            else
            {
                throw new System.IO.FileNotFoundException($"File does not exist: {filenameFrom}");
            }
        }
    }
}
