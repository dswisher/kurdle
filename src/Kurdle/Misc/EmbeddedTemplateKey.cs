using RazorEngine.Templating;


namespace Kurdle.Misc
{
    public class EmbeddedTemplateKey : BaseTemplateKey
    {
        public EmbeddedTemplateKey(string name, string embeddedPath, ResolveType resolveType, ITemplateKey context)
            : base(name, resolveType, context)
        {
            EmbeddedPath = embeddedPath;
        }

        public string EmbeddedPath { get; private set; }

        public override string GetUniqueKeyString()
        {
            return EmbeddedPath;
        }
    }
}
