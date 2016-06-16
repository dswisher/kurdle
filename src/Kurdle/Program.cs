using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Markdig;

namespace Kurdle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Console.WriteLine("Hello, world!");

            var result = Markdown.ToHtml("This is a text with some *emphasis*");
            Console.WriteLine(result);   // prints: <p>This is a text with some <em>emphasis</em></p>

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press a key to exit...");
                Console.ReadKey(true);
            }
        }
    }
}
