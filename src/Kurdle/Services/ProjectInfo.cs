using System;
using System.IO;
using System.Linq;
using Kurdle.Misc;

namespace Kurdle.Services
{
    public interface IProjectInfo
    {
        void Init();

        DirectoryInfo OutputDirectory { get; }
    }


    public class ProjectInfo : IProjectInfo
    {
        public void Init()
        {
            // Walk up the directory tree until we find the project info file
            FileInfo projectFile = FindProjectFile();
            Console.WriteLine("   -> Project file: {0}", projectFile.FullName);

            // Parse the project info
            ParseProjectInfo(projectFile);

            // Parse the user-specific overrides, if any
            FileInfo userFile = SearchDirectoryForUserFile(projectFile.Directory);
            if (userFile != null)
            {
                Console.WriteLine("   -> User file:    {0}", projectFile.FullName);
                ParseProjectInfo(userFile);
            }

            Console.WriteLine();
            Console.WriteLine("   -> Output: {0}", (OutputDirectory == null ? "(null)" : OutputDirectory.FullName));
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

                        default:
                            throw new ProjectException("Invalid project setting ({2}), line {0} of {1}.",
                                lineNumber, projectFile.FullName, name ?? "[null]");
                    }
                }
            }
        }



        public DirectoryInfo OutputDirectory { get; private set; }


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



        private FileInfo FindProjectFile()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);

            do
            {
                // Look for the file
                FileInfo info = SearchDirectoryForProjectFile(dir);

                if (info != null)
                {
                    return info;
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



        private FileInfo SearchDirectoryForUserFile(DirectoryInfo dir)
        {
            var info = dir.GetFiles("project.user").FirstOrDefault();

            // TODO - search for other names?

            return info;
        }
    }
}
