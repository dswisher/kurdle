using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurdle.Services;

namespace Kurdle.Generation
{
    public abstract class AbstractPageGenerator
    {
        protected readonly DocumentEntry _entry;

        protected AbstractPageGenerator(DocumentEntry entry)
        {
            _entry = entry;
        }


        public void Generate()
        {
            // TODO!
        }
    }
}
