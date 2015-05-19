using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurdle.Services;

namespace Kurdle.Generation
{
    public class MarkDownPageGenerator : AbstractPageGenerator
    {
        public MarkDownPageGenerator(DocumentEntry entry) : base(entry)
        {
        }
    }
}
