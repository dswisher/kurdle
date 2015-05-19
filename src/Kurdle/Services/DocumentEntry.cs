

using System;
using System.IO;
using System.Text;
using Kurdle.MetaData;
using Kurdle.Misc;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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



        public void ScanHeader()
        {
            StringBuilder builder = new StringBuilder();

            using (var reader = Info.OpenText())
            {
                int lineNumber = 0;
                string data;
                bool inHeader = false;
                while ((data = reader.ReadLine()) != null)
                {
                    lineNumber += 1;
                    if (!inHeader)
                    {
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            if (data == "---")
                            {
                                inHeader = true;
                            }
                            else
                            {
                                throw new ProjectException("Expected header start at line {0} of {1}, but found '{2}' instead.",
                                    lineNumber, Info.Name, data);
                            }
                        }
                    }
                    else
                    {
                        if (data == "---")
                        {
                            break;
                        }

                        builder.AppendLine(data);
                    }
                }
            }

            // Send the header through the YAML parser...
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

            using (var reader = new StringReader(builder.ToString()))
            {
                var meta = deserializer.Deserialize<DocumentMetaData>(reader);

                // TODO - debug code - rip this out
                Console.WriteLine("File: {0}", Info.Name);
                Console.WriteLine("   Title: {0}", meta.Title);
                Console.WriteLine("   Template: {0}", meta.Template);
            }
        }
    }
}
