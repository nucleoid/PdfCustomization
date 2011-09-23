using System.IO;

namespace PdfCustomization.Tasks
{
    public class PdfTasks
    {
        public Stream GeneratePdfStream(string url)
        {
            var document = new PdfDocument { Url = url };
            var output = new PdfOutput {OutputStream = new MemoryStream()};
            PdfConvert.ConvertHtmlToPdf(document, output);
            output.OutputStream.Position = 0;
            return output.OutputStream;
        }
    }
}