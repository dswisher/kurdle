using System;
using System.IO;
using System.Xml;
using CommonMark;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public class MarkDownPageGenerator : AbstractPageGenerator
    {
        public MarkDownPageGenerator(IRazorEngineService razorEngine, IProjectInfo projectInfo, DocumentEntry entry)
            : base(razorEngine, projectInfo, entry)
        {
        }



        protected override string GetPageContent(TextReader reader)
        {
            const string wrapper = "faux-wrapper";
            const string wrapperStart = "<" + wrapper + ">";
            const string wrapperEnd = "</" + wrapper + ">";

            // Process markdown to HTML...
            var rawContent = CommonMarkConverter.Convert(reader.ReadToEnd());

            // Post-process the HTML...
            XmlDocument xml = new XmlDocument();

            try
            {
                xml.LoadXml(wrapperStart + rawContent + wrapperEnd);
            }
            catch (Exception)
            {
                const string name = "invalid-html.xml";

                Console.WriteLine("Raw content is invalid XML.");
                DumpContent(rawContent, name);
                throw;
            }

            // TODO - post processing

            // ReSharper disable once PossibleNullReferenceException
            var content = xml.SelectSingleNode(wrapper).InnerXml;

            // Return what we've got...
            return content;
        }



        private void DumpContent(string content, string name)
        {
            try
            {
                string path = Path.Combine(_projectInfo.OutputDirectory.FullName, name);

                File.WriteAllText(path, content);

                Console.WriteLine("Invalid content written to {0}.", path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception attempting to dump invalid XML.");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
