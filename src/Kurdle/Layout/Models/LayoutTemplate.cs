using System.Collections.Generic;
using HandlebarsDotNet;

namespace Kurdle.Layout.Models
{
    public class LayoutTemplate
    {
        private readonly Dictionary<string, object> viewModel = new();
        private readonly HandlebarsTemplate<object, object> template;

        public LayoutTemplate(HandlebarsTemplate<object, object> template)
        {
            this.template = template;
        }


        public string Apply(string body)
        {
            viewModel.Add("body", body);

            return template(viewModel);
        }


        public void SetValue(string name, object value)
        {
            viewModel.Add(name, value);
        }
    }
}
