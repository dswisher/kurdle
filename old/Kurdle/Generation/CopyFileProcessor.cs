

namespace Kurdle.Generation
{
    public class CopyFileProcessor : AbstractFileProcessor
    {
        public CopyFileProcessor(IProjectInfo projectInfo, DocumentEntry entry)
            : base(projectInfo, entry)
        {
        }


        public override void Process(bool dryRun)
        {
            if (!dryRun)
            {
                var outputInfo = GetOutputInfo();

                MakeOutputDir();

                if (outputInfo.Exists)
                {
                    outputInfo.Delete();
                }

                _entry.Info.CopyTo(outputInfo.FullName);
            }
        }
    }
}
