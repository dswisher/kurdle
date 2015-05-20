

namespace Kurdle.Generation
{
    public class DocumentMetaData
    {
        static DocumentMetaData()
        {
            Empty = new DocumentMetaData();
        }


        public DocumentMetaData()
        {
            // Set defaults
            Template = "Document";
        }

        public string Template { get; set; }
        public string Title { get; set; }


        public static DocumentMetaData Empty { get; private set; }
    }
}
