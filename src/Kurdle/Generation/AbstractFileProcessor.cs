
using System.IO;

namespace Kurdle.Generation
{
    public abstract class AbstractFileProcessor : IFileProcessor
    {
        protected readonly IProjectInfo _projectInfo;
        protected readonly DocumentEntry _entry;
        private readonly DirectoryInfo _outputDirectoryInfo;


        protected AbstractFileProcessor(IProjectInfo projectInfo, DocumentEntry entry)
        {
            _projectInfo = projectInfo;
            _entry = entry;

            var subdir = Path.Combine(_projectInfo.OutputDirectory.FullName, _entry.SubDirectory);
            _outputDirectoryInfo = new DirectoryInfo(subdir);
        }


        public abstract void Process(bool dryRun);


        protected FileInfo GetOutputInfo(string newExtension = null)
        {
            var outputName = (newExtension == null) ? _entry.Info.Name : Path.ChangeExtension(_entry.Info.Name, newExtension);
            var path = Path.Combine(_outputDirectoryInfo.FullName, outputName);
            var file = new FileInfo(Path.GetFullPath(path));

            return file;
        }



        protected void MakeOutputDir()
        {
            MakeDir(_outputDirectoryInfo);
        }



        private void MakeDir(DirectoryInfo dir)
        {
            if (!dir.Exists)
            {
                MakeDir(dir.Parent);

                dir.Create();
            }
        }
    }
}
