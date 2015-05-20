
using System.IO;

namespace Kurdle.Generation
{
    public abstract class AbstractFileProcessor : IFileProcessor
    {
        protected readonly IProjectInfo _projectInfo;
        protected readonly DocumentEntry _entry;


        protected AbstractFileProcessor(IProjectInfo projectInfo, DocumentEntry entry)
        {
            _projectInfo = projectInfo;
            _entry = entry;
        }


        public abstract void Process(bool dryRun);


        private DirectoryInfo GetOutputDirectory()
        {
            // TODO - build the output dir on construction and save it
            var subdir = Path.Combine(_projectInfo.OutputDirectory.FullName, _entry.SubDirectory);

            return new DirectoryInfo(subdir);
        }



        protected FileInfo GetOutputInfo(string newExtension = null)
        {
            var outputName = (newExtension == null) ? _entry.Info.Name : Path.ChangeExtension(_entry.Info.Name, newExtension);
            var path = Path.Combine(GetOutputDirectory().FullName, outputName);
            var file = new FileInfo(Path.GetFullPath(path));

            return file;
        }



        protected void MakeOutputDir()
        {
            MakeDir(GetOutputDirectory());
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
