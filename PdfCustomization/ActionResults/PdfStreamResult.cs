using System.IO;
using System.Web.Mvc;

namespace PdfCustomization.ActionResults
{
    public class PdfStreamResult : FileStreamResult
    {
        public const string PdfContentType = "application/pdf";

        public PdfStreamResult(Stream fileStream)
            : base(fileStream, PdfContentType)
        {
        }
    }
}