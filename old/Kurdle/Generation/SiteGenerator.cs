using System;


namespace Kurdle.Generation
{
    public interface ISiteGenerator
    {
        void Generate(bool dryRun = false);
    }



    public class SiteGenerator : ISiteGenerator
    {
        private readonly IProjectInfo _projectInfo;
        private readonly IPageGeneratorFactory _pageGeneratorFactory;


        public SiteGenerator(IProjectInfo projectInfo,
                             Func<IProjectInfo, IPageGeneratorFactory> pageGeneratorFactory)
        {
            _projectInfo = projectInfo;
            _pageGeneratorFactory = pageGeneratorFactory(projectInfo);
        }


        public void Generate(bool dryRun)
        {
            Console.WriteLine("Generating site...");

            // Make sure the output directory exists
            if (!dryRun)
            {
                if (!_projectInfo.OutputDirectory.Exists)
                {
                    _projectInfo.OutputDirectory.Create();
                }
            }

            // Generate all the documents...
            foreach (var doc in _projectInfo.Documents)
            {
                var generator = _pageGeneratorFactory.Create(doc);

                generator.Process(dryRun);
            }
        }
    }
}
