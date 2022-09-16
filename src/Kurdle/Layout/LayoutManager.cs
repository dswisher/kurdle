using System.IO;
using HandlebarsDotNet;
using Kurdle.Layout.Models;

namespace Kurdle.Layout
{
    public class LayoutManager : ILayoutManager
    {
        // TODO - seems like there should be a "search path" for templates, not just one directory
        public string TemplateDirectory { get; set; }


        public LayoutTemplate GetTemplate(string name)
        {
            // TODO - caching! Need to return a new LayoutTemplate each time, but can reuse the HB template

            var templatePath = Path.Join(TemplateDirectory, $"{name}.hbs");
            var source = File.ReadAllText(templatePath);

            var template = Handlebars.Compile(source);

            return new LayoutTemplate(template);
        }
    }
}
