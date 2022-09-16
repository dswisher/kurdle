using HandlebarsDotNet;
using Kurdle.Layout.Models;

namespace Kurdle.Layout
{
    public interface ILayoutManager
    {
        string TemplateDirectory { get; set; }

        LayoutTemplate GetTemplate(string name);
    }
}
