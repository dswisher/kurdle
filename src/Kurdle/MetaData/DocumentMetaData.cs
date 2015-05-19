

namespace Kurdle.MetaData
{
    public class DocumentMetaData
    {
        public DocumentMetaData()
        {
            // Set defaults
            Template = "Article";
        }

        public string Template { get; set; }
        public string Title { get; set; }
    }
}
