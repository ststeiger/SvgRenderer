
namespace FontConfigTest
{


    public class MimeMagicTest
    {


        public static void Test()
        {
            string originalMagicFile = System.IO.Path.Combine(System.Environment.CurrentDirectory, "magic_original"),
                jsonMagicFile = System.IO.Path.Combine(System.Environment.CurrentDirectory, "magic_json");

            MimeMagicSharp.MimeMagicSharp.ConvertFromOriginalToJson("magic_original", "magic_json_convert_test");

            //  Read and detect
            using (MimeMagicSharp.MimeMagicSharp ms =
                new MimeMagicSharp.MimeMagicSharp(MimeMagicSharp.EMagicFileType.Json, jsonMagicFile))
            {
                try
                {

                    foreach (MimeMagicSharp.MimeTypeGuess mimeTypeGuess in ms.AssumeMimeType(MimeMagicSharp.EMimeTypeBy.Content,
                        System.IO.Path.Combine(System.Environment.CurrentDirectory, "Newtonsoft.Json.xml")))
                    {
                        //  Iterate over results
                    }

                }
                catch (System.Exception ex)
                {
                    //  Hande errors here
                }
            }

        }


    }


}
