using System;
using Kurdle.Services;

namespace Kurdle.Generation
{
    public interface IPageGeneratorFactory
    {
        AbstractPageGenerator Create(DocumentEntry entry);
    }



    public class PageGeneratorFactory : IPageGeneratorFactory
    {

        public AbstractPageGenerator Create(DocumentEntry entry)
        {
            // Construct the page generator itself...
            AbstractPageGenerator generator;
            switch (entry.Kind)
            {
                case DocumentKind.MarkDown:
                    generator = new MarkDownPageGenerator(entry);
                    break;

                case DocumentKind.AsciiDoc:
                    generator = new AsciiDocPageGenerator(entry);
                    break;

                default:
                    throw new NotImplementedException("Pages of kind '" + entry.Kind + "' are not yet implemented.");
            }

            // Create and populate the model...
            // TODO

            // Return what we hath wrought...
            return generator;
        }

    }
}
