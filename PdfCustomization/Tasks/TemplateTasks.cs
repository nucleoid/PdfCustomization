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

        public string ParseTemplate(string templateText, UserModel model)
        {
            if (!_initialized)
            {
                HostingEnvironment.RegisterVirtualPathProvider(new RazorVirtualPathProvider());
                _initialized = true;
            }
                
            return Razor.Parse(templateText, model);
        }
    }
}