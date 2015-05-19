using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurdle.Services;

namespace Kurdle.Generation
{
    public class AsciiDocPageGenerator : AbstractPageGenerator
    {
        public AsciiDocPageGenerator(DocumentEntry entry) : base(entry)
        {
        }
    }
}
