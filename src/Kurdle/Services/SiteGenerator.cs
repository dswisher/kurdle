using System;
using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Configuration;


namespace Kurdle.Services
{
    public interface ISiteGenerator
    {
        void Generate(IProjectInfo projectInfo, bool dryRun);
    }



    public class SiteGenerator : ISiteGenerator
    {
		private readonly IRazorEngineService _razorEngine;


		public SiteGenerator()
		{
			var config = new TemplateServiceConfiguration();

			config.TemplateManager = new EmbeddedTemplateManager("Kurdle.Templates");

			// TODO - configure the instance

			_razorEngine = RazorEngineService.Create(config);
		}


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
					_razorEngine.Compile("Index");

					var result = _razorEngine.Run("Index", null, new { Name = "Homer" });

					writer.Write(result);

//                    writer.WriteLine("<html><head><title>My blog!</title></head><body>");
//                    writer.WriteLine("<p>Coming soon!</p>");
//                    writer.WriteLine("<body></html>");
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
