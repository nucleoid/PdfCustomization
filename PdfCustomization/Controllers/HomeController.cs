using System.IO;
using System.Web.Mvc;
using PdfCustomization.ActionResults;
using PdfCustomization.Models;
using PdfCustomization.Tasks;

namespace PdfCustomization.Controllers
{
    public class HomeController : Controller
    {
        private TemplateTasks _templateTasks;
        private PdfTasks _pdfTasks;
        private static string _directory;

        public HomeController()
        {
            _templateTasks = new TemplateTasks();
            _pdfTasks = new PdfTasks();
            _directory = Directory.GetParent(typeof(HomeController).Assembly.CodeBase.Remove(0, 8).Replace('/', '\\')).ToString();
        }

        [Authorize]
        public ActionResult Index()
        {
            var defaultTemplate = _templateTasks.GenerateDefaultTemplate();
            return View(new TemplateModel{Template = defaultTemplate});
        }

        [Authorize]
        public ActionResult ContractAgreement()
        {
            var template = _templateTasks.GenerateContractAgreementTemplate();
            return View(new TemplateModel { Template = template });
        }

        [Authorize]
        public ActionResult ExportContract()
        {
            var template = _templateTasks.GenerateExportContractTemplate();
            return View(new TemplateModel { Template = template });
        }

        [Authorize]
        public ActionResult ReexportContract()
        {
            var template = _templateTasks.GenerateReexportContractTemplate();
            return View(new TemplateModel { Template = template });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTemplate(TemplateModel model)
        {
            var generatedText = _templateTasks.ParseTemplate(model.Template, GenerateModel());
            StoredTemplate = generatedText;
            var action = Url.Action("UrlToGenerate", new {userName = User.Identity.Name});
            var fullUrl = string.Format("http://{0}{1}", Request.Url.Authority, action);
            var pdfStream = _pdfTasks.GeneratePdfStream(fullUrl);
            return new PdfStreamResult(pdfStream) {FileDownloadName = "Agreement.pdf"};
        }

        [ValidateInput(false)]
        public ActionResult UrlToGenerate(string userName)
        {
            UserName = userName;
            var content = StoredTemplate;
            return new ContentResult { Content = content, ContentType = "text/html" };
        }

        public ActionResult About()
        {
            return View();
        }

        private string UserName { get; set; }
        private string StoredTemplate
        {
            get
            {
                var template = System.IO.File.ReadAllText(Path.Combine(_directory, UserName));
                return template;
            }
            set { System.IO.File.WriteAllText(Path.Combine(_directory, User.Identity.Name), value); }
        }

        private UserModel GenerateModel()
        {
            var model = new UserModel { Name = User.Identity.Name };
            model.Commitments.Add(new QuarterlyCommitment { QuantityOfSugar = 250000, Quarter = "1/1/2009 – 3/31/2009" });
            model.Commitments.Add(new QuarterlyCommitment { QuantityOfSugar = 250000, Quarter = "3/1/2009 – 6/30/2009" });
            model.Commitments.Add(new QuarterlyCommitment { QuantityOfSugar = 250000, Quarter = "7/1/2009 – 9/30/2009" });
            model.Commitments.Add(new QuarterlyCommitment { QuantityOfSugar = 750000, Quarter = "10/1/2009 – 12/31/2009" });

            model.Prices.Add(new Price {DeliveryLocation = "RRV", UnitPrice = 34.00m, Mode = Mode.Magic, PackageSizeAndType = "cwt", State = "MN", TotalPrice = 340.00m});
            model.Prices.Add(new Price { DeliveryLocation = "RRV", UnitPrice = 35.00m, Mode = Mode.Offline, PackageSizeAndType = "cwt", State = "WY", TotalPrice = 350.00m });
            model.Prices.Add(new Price { DeliveryLocation = "RRV", UnitPrice = 35.00m, Mode = Mode.Magic, PackageSizeAndType = "cwt", State = "ND", TotalPrice = 350.00m });
            model.Prices.Add(new Price { DeliveryLocation = "RRV", UnitPrice = 34.00m, Mode = Mode.Online, PackageSizeAndType = "cwt", State = "MN", TotalPrice = 340.00m });

            return model;
        }
    }
}
