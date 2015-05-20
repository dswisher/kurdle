

namespace Kurdle.Generation
{
    public class CopyFileProcessor : AbstractFileProcessor
    {
        private readonly DocumentEntry _entry;

        public CopyFileProcessor(IProjectInfo projectInfo, DocumentEntry entry)
            : base(projectInfo, entry)
        {
            _entry = entry;
        }


        public override void Process(bool dryRun)
        {
            if (!dryRun)
            {
                MakeOutputDir();
                _entry.Info.CopyTo(GetOutputInfo().FullName);
            }
        }
    }
}
