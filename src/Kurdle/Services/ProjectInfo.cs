using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            // Parse the project info
            ParseProjectInfo(projectFile);

            // Parse the user-specific overrides, if any
            // TODO
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
                            OutputDirectory = new DirectoryInfo(value);
                            break;

                        default:
                            throw new ProjectException("Invalid project setting, line {0} of {1}.", lineNumber, projectFile.FullName);
                    }
                }
            }
        }



        public DirectoryInfo OutputDirectory { get; private set; }


        private FileInfo FindProjectFile()
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);

            do
            {
                Console.WriteLine("Looking in {0}...", dir.FullName);

                // Look for the file
                FileInfo info = SearchDirectory(dir);

                if (info != null)
                {
                    return info;
                }

                // Walk up to the parent
                dir = dir.Parent;
            } while (dir != null);

            throw new ProjectException("Could not locate project info file.");
        }



        private FileInfo SearchDirectory(DirectoryInfo dir)
        {
            var info = dir.GetFiles("project.dat").FirstOrDefault();

            // TODO - search for other names?

            return info;
        }
    }
}
