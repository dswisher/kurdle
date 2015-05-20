using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurdle.Generation
{
    public class CopyWorker : IPageGenerator
    {
        private readonly DocumentEntry _entry;

        public CopyWorker(DocumentEntry entry)
        {
            _entry = entry;
        }


        public void Generate(bool dryRun)
        {
            // TODO - copy the content
        }
    }
}
