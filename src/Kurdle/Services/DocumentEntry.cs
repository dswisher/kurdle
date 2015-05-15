

using System.IO;

namespace Kurdle.Services
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

        public DocumentEntry(DocumentKind kind, FileInfo info)
        {
            Kind = kind;
            Info = info;
        }
    }
}
