using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PdfCustomization.ActionResults;
using PdfCustomization.Models;
using PdfCustomization.Tasks;

namespace PdfCustomization.Controllers
{
    public class HomeController : Controller
    {
        private TemplateTasks _templateTasks;
        private PdfTasks _pdfTasks;

        public HomeController()
        {
            _templateTasks = new TemplateTasks();
            _pdfTasks = new PdfTasks();
        }

        [Authorize]
        public ActionResult Index()
        {
            var defaultTemplate = _templateTasks.GenerateDefaultTemplate();
            return View(new TemplateModel{Template = defaultTemplate});
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTemplate(TemplateModel model)
        {
            var generatedText = _templateTasks.ParseTemplate(model.Template, new UserModel {Name = User.Identity.Name});
            var converted = HttpUtility.UrlEncode(generatedText);
            var action = Url.Action("UrlToGenerate", new { content = converted });
            var fullUrl = string.Format("http://{0}{1}", Request.Url.Authority, action);
            var pdfStream = _pdfTasks.GeneratePdfStream(fullUrl);
            return new PdfStreamResult(pdfStream) {FileDownloadName = "test.pdf"};
        }

        [ValidateInput(false)]
        public ActionResult UrlToGenerate(string content)
        {
            var converted = HttpUtility.UrlDecode(content);
            return new ContentResult { Content = converted, ContentType = "text/html" };
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
