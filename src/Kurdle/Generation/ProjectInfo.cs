using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kurdle.Misc;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kurdle.Generation
{
    public interface IProjectInfo
    {
        void Init();

        DirectoryInfo OutputDirectory { get; }
        string SiteName { get; }
        IEnumerable<DocumentEntry> Documents { get; }
    }



    public class ProjectInfo : IProjectInfo
    {
        private readonly List<DocumentEntry> _documents = new List<DocumentEntry>();
        private DirectoryInfo _root;

        public DirectoryInfo OutputDirectory { get; private set; }
        public string SiteName { get; private set; }
        public IEnumerable<DocumentEntry> Documents { get { return _documents; } }


        public void Init()
        {
            // Walk up the directory tree until we find the project info file
            FindAndParseProjectFile();

            // Parse the user-specific overrides, if any
            FindAndParseUserFile();

            // Scan for all the document files
            ScanForDocuments(_root);

            // Dump out some info
            Console.WriteLine();
            Console.WriteLine("   -> Output: {0}", (OutputDirectory == null ? "(null)" : OutputDirectory.FullName));
        }



        private void ScanForDocuments(DirectoryInfo dir)
        {
            foreach (var file in dir.GetFiles())
            {
                var extension = Path.GetExtension(file.Name).Substring(1).ToLower();

                DocumentKind kind;
                switch (extension)
                {
                    case "md":
                        kind = DocumentKind.MarkDown;
                        break;

                    case "txt":
                        kind = DocumentKind.AsciiDoc;
                        break;

                    default:
                        continue;
                }

                var entry = new DocumentEntry(kind, file, ScanHeader(file));

                _documents.Add(entry);
            }

            foreach (var subdir in dir.GetDirectories())
            {
                ScanForDocuments(subdir);
            }
        }



        private DocumentMetaData ScanHeader(FileInfo info)
        {
            StringBuilder builder = new StringBuilder();

            using (var reader = info.OpenText())
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
                                    lineNumber, info.Name, data);
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
                return deserializer.Deserialize<DocumentMetaData>(reader);
            }
        }



        private void ParseProjectInfo(FileInfo projectFile)
        {
            using (var reader = projectFile.OpenText())
            {
                int lineNumber = 0;
                string data;
                while ((data = reader.ReadLine()) != null)
                {
                    lineNumber += 1;

                    data = data.Trim();

                    if (data.Length == 0)
                    {
                        continue;
                    }

                    int pos = data.IndexOf(':');
                    string name = null;
                    string value = null;
                    if (pos > 0)
                    {
                        name = data.Substring(0, pos).Trim().ToLower();
                        value = data.Substring(pos + 1).Trim();
                    }

                    switch (name)
                    {
                        case "output":
                            OutputDirectory = ParseDirectory(projectFile, value);
                            break;

                        case "sitename":
                            SiteName = value;
                            break;

                        default:
                            throw new ProjectException("Invalid project setting ({2}), line {0} of {1}.",
                                lineNumber, projectFile.FullName, name ?? "[null]");
                    }
                }
            }
        }



        private DirectoryInfo ParseDirectory(FileInfo root, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            // Relative path?
            if (!Path.IsPathRooted(path) && (root.Directory != null))
            {
                string rel = Path.Combine(root.Directory.FullName, path);

                return new DirectoryInfo(rel);
            }

            // Absolute path
            return new DirectoryInfo(path);
        }



        private void FindAndParseProjectFile()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);

            do
            {
                // Look for the file
                FileInfo info = SearchDirectoryForProjectFile(dir);

                if (info != null)
                {
                    ParseProjectInfo(info);
                    _root = info.Directory;
                    Console.WriteLine("   -> Project file: {0}", info.FullName);
                    return;
                }

                // Walk up to the parent
                dir = dir.Parent;
            } while (dir != null);

            throw new ProjectException("Could not locate project info file.");
        }



        private FileInfo SearchDirectoryForProjectFile(DirectoryInfo dir)
        {
            var info = dir.GetFiles("project.dat").FirstOrDefault();

            // TODO - search for other names?

            return info;
        }



        private void FindAndParseUserFile()
        {
            var info = _root.GetFiles("project.user").FirstOrDefault();

            // TODO - search for other names?

            if (info != null)
            {
                Console.WriteLine("   -> User file:    {0}", info.FullName);
                ParseProjectInfo(info);
            }
        }
    }
}
