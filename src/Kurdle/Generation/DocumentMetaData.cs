

namespace Kurdle.Generation
{
    public class DocumentMetaData
    {
        public DocumentMetaData()
        {
            // Set defaults
            Template = "Document";
        }

        public string Template { get; set; }
        public string Title { get; set; }
    }
}
