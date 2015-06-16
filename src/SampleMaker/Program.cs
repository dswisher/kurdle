using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Kurdle SampleMaker");

            if (args.Length == 0)
            {
                Console.WriteLine("You must specify the output directory.");
                return;
            }

            var outputDir = new DirectoryInfo(args[0]);
            if (outputDir.Exists)
            {
                Console.WriteLine("Cleaning up old output directory: {0}", outputDir.FullName);
                outputDir.Delete(true);
            }

            outputDir.Create();
            
            int numArticles = 10;
            if (args.Length > 1)
            {
                numArticles = int.Parse(args[1]);
            }

            Console.WriteLine("Generating sample site with {0} random articles.", numArticles);

            // Create templates
            // TODO

            // Create the documents
            Random chaos = new Random(316);
            for (int i = 0; i < numArticles; i++)
            {
                CreateDocument(outputDir, chaos);
            }

            // TODO - implement me!
            Console.WriteLine("TBD!");
        }



        private static void CreateDocument(DirectoryInfo outputDir, Random chaos)
        {
            // TODO
        }
    }
}
