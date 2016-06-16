using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurdle.Misc;

namespace Kurdle.Model
{
    public class Site
    {
        private readonly Options _options;
        private readonly List<Page> _pages = new List<Page>();
        private readonly List<StaticFile> _staticFiles = new List<StaticFile>();
        private readonly HashSet<string> _staticExtensions = new HashSet<string>();



        public Site(Options options)
        {
            _options = options;

            // TODO - make this list customizable via options
            _staticExtensions.Add("css");
            _staticExtensions.Add("gif");
            _staticExtensions.Add("jpg");
            _staticExtensions.Add("js");
            _staticExtensions.Add("png");
        }


        public void Process()
        {
            // TODO - see https://github.com/jekyll/jekyll/wiki/How-Jekyll-works
            //      - http://jekyllrb.com/docs/plugins/

            // TODO - need to fire hooks!
            Reset();
            Read();
            // Generate();
            // Render();
            // Cleanup();
            // Write();

            throw new KurdleException("Site.Process() is only partially implemented.");
        }



        public void Reset()
        {
            _pages.Clear();
            _staticFiles.Clear();
        }



        public void Read()
        {
            // TODO - init exclude-list from options
            var excludeList = ExcludeList.Empty;

            // TODO - add include-list (from options)

            var source = new DirectoryInfo(_options.Source);

            ScanForFiles(source, excludeList);

            // TODO!
        }



        private void ScanForFiles(DirectoryInfo dir, ExcludeList excludeList)
        {
            if (_options.Verbose)
            {
                Console.WriteLine("...scanning {0}...", dir.Name);
            }

            // Look for a .gitignore file in this directory
            var gitIgnore = dir.GetFiles(".gitignore").FirstOrDefault();
            if (gitIgnore != null)
            {
                if (_options.Verbose)
                {
                    Console.WriteLine("   ...parsing {0}...", gitIgnore.Name);
                }
                excludeList = excludeList.Parse(gitIgnore);
            }

            foreach (var file in dir.GetFiles())
            {
                // Skip ignored files
                if (excludeList.IsIgnored(file))
                {
                    // TODO - check include-list
                    continue;
                }

                // We base decisions on the file extension, so grab it and normalize it...
                var extension = Path.GetExtension(file.Name).Substring(1).ToLower();

                // Is this a static file?
                if (_staticExtensions.Contains(extension))
                {
                    if (_options.Verbose)
                    {
                        Console.WriteLine("   ...found static file {0}...", file.Name);
                    }
                    // TODO - create a static file entry
                    continue;
                }

                // TODO - attempt to read the front-matter and if successful, create a Page entry
                if (_options.Verbose)
                {
                    Console.WriteLine("   ...found content {0}...", file.Name);
                }
            }

            foreach (var subdir in dir.GetDirectories())
            {
                if (excludeList.IsIgnored(subdir))
                {
                    continue;
                }

                ScanForFiles(subdir, excludeList);
            }
        }
    }
}
