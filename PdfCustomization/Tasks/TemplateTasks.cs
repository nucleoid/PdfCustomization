using System;
using System.IO;
using System.Web.Hosting;
using PdfCustomization.Models;
using RazorEngine;
using RazorEngine.Web;

namespace PdfCustomization.Tasks
{
    public class TemplateTasks
    {
        private static bool _initialized;

        public string GenerateDefaultTemplate()
        {
            var templateText = new StreamReader(GetType().Assembly.GetManifestResourceStream("PdfCustomization.Templates.Default.cshtml")).ReadToEnd();
            return templateText;
        }

        public string ParseTemplate(string templateText, UserModel userModel)
        {
            if (!_initialized)
            {
                HostingEnvironment.RegisterVirtualPathProvider(new RazorVirtualPathProvider());
                _initialized = true;
            }
            var model = GenerateModel(userModel);    
            return Razor.Parse(templateText, model);
        }

        public dynamic GenerateModel(UserModel userModel)
        {
            var date = DateTime.Now;
            return new
                       {
                           month = date.ToString("MMM"), 
                           day = date.Day, 
                           year = date.ToString("yyyy"), 
                           effectiveDate = date.ToString("MMM dd, yyyy"),
                           Customer = userModel.Name,
                           Address = "210 N 5th Ave Fargo, ND 58103",
                           Contact = "Mr. Important Guy",
                           fax = "701-203-5689",
                           Buyer = "Nick Swardson", 
                           userModel.Commitments, 
                           userModel.Prices
                       };
        }
    }
}