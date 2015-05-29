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
        void Init(bool verbose);

        DirectoryInfo OutputDirectory { get; }
        DirectoryInfo TemplateDirectory { get; }
        string SiteName { get; }
        IEnumerable<DocumentEntry> Documents { get; }
        DirectoryInfo Root { get; }

        bool IsIgnored(string path);
    }



    public class ProjectInfo : IProjectInfo
    {
        private readonly List<DocumentEntry> _documents = new List<DocumentEntry>();
        private readonly Deserializer _deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

        public bool Verbose { get; private set; }

        public DirectoryInfo OutputDirectory { get; private set; }
        public DirectoryInfo TemplateDirectory { get; private set; }
        public string SiteName { get; private set; }
        public IEnumerable<DocumentEntry> Documents { get { return _documents; } }
        public DirectoryInfo Root { get; private set; }


        public void Init(bool verbose)
        {
            Verbose = verbose;

            // Walk up the directory tree until we find the project info file
            FindAndParseProjectFile();

            // Parse the user-specific overrides, if any
            FindAndParseUserFile();

            // Scan for all the document files
            ScanForDocuments(Root, string.Empty, IgnoreList.Empty);

            // Dump out some info
            if (Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("   -> Root Dir:     {0}", Root.FullName);
                Console.WriteLine("   -> Template Dir: {0}", TemplateDirectory == null ? "(null)" : TemplateDirectory.FullName);
                Console.WriteLine("   -> Output Dir:   {0}", OutputDirectory == null ? "(null)" : OutputDirectory.FullName);
            }
        }



        public bool IsIgnored(string path)
        {
            // TODO!
            return false;
        }



        private void ScanForDocuments(DirectoryInfo dir, string path, IgnoreList ignoreList)
        {
            if (Verbose)
            {
                Console.WriteLine("...scanning {0}...", path);
            }

            foreach (var file in dir.GetFiles())
            {
                // TODO - scan for .gitignore first, in a separate loop?
                // Special case - .gitignore
                if (file.Name == ".gitignore")
                {
                    ignoreList = ignoreList.Parse(file);
                    continue;
                }

                // Is this file ignored?
                if (ignoreList.IsIgnored(file))
                {
                    continue;
                }

                // Handle the "normal" cases...
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

                    case "js":
                        kind = DocumentKind.Script;
                        break;

                    case "css":
                        kind = DocumentKind.Style;
                        break;

                    case "gif":
                    case "jpg":
                    case "png":
                        kind = DocumentKind.Image;
                        break;

                    default:
                        continue;
                }

                var entry = new DocumentEntry(kind, file, IsStatic(kind) ? DocumentMetaData.Empty : ScanHeader(file), path);

                _documents.Add(entry);
            }

            foreach (var subdir in dir.GetDirectories())
            {
                if (ignoreList.IsIgnored(subdir))
                {
                    continue;
                }

                var subpath = Path.Combine(path, subdir.Name);

                ScanForDocuments(subdir, subpath, ignoreList);
            }
        }



        private bool IsStatic(DocumentKind kind)
        {
            switch (kind)
            {
                case DocumentKind.Script:
                case DocumentKind.Style:
                case DocumentKind.Image:
                    return true;

                default:
                    return false;
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
            using (var reader = new StringReader(builder.ToString()))
            {
                return _deserializer.Deserialize<DocumentMetaData>(reader);
            }
        }



        private void ParseProjectInfo(FileInfo projectFile)
        {
            using (var reader = projectFile.OpenText())
            {
                var settings = _deserializer.Deserialize<Settings>(reader);

                SiteName = settings.SiteName;
                TemplateDirectory = ParseDirectory(projectFile, settings.TemplateDir);
                OutputDirectory = ParseDirectory(projectFile, settings.Output);
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
                    Root = info.Directory;
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
            var info = Root.GetFiles("project.user").FirstOrDefault();

            // TODO - search for other names?

            if (info != null)
            {
                Console.WriteLine("   -> User file:    {0}", info.FullName);
                ParseProjectInfo(info);
            }
        }



        public class Settings
        {
            public string Output { get; set; }
            public string SiteName { get; set; }
            public string TemplateDir { get; set; }
        }
    }
}
