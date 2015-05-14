using System.IO;


namespace Kurdle.Services
{
    public interface ISiteGenerator
    {
        void Generate(IProjectInfo projectInfo, bool dryRun);
    }



    public class SiteGenerator : ISiteGenerator
    {
        public void Generate(IProjectInfo projectInfo, bool dryRun)
        {
            // TODO - this is just a quick hack to get something going

            // Make sure the output directory exists
            if (!dryRun)
            {
                if (!projectInfo.OutputDirectory.Exists)
                {
                    projectInfo.OutputDirectory.Create();
                }
            }

            // Generate the home page
            if (!dryRun)
            {
                var file = GetFileInfo(projectInfo, "index.html");

                using (var writer = file.CreateText())
                {
                    writer.WriteLine("<html><head><title>My blog!</title></head><body>");
                    writer.WriteLine("<p>Coming soon!</p>");
                    writer.WriteLine("<body></html>");
                }
            }
        }



        private FileInfo GetFileInfo(IProjectInfo projectInfo, string filePath)
        {
            var path = Path.Combine(projectInfo.OutputDirectory.FullName, filePath);

            return new FileInfo(Path.GetFullPath(path));
        }
    }
}
