using System.IO;

namespace Kurdle.Generation
{
    public enum DocumentKind
    {
        MarkDown,
        AsciiDoc
    }



    public class DocumentEntry
    {
        public DocumentKind Kind { get; private set; }
        public FileInfo Info { get; private set; }
        public string Template { get; private set; }

        public DocumentEntry(DocumentKind kind, FileInfo info, DocumentMetaData metaData)
        {
            Kind = kind;
            Info = info;
            Template = metaData.Template;
        }
    }
}
