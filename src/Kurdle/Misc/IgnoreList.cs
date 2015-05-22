using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Kurdle.Misc
{
    public class IgnoreList
    {
        private readonly List<Regex> _ignoreMasks = new List<Regex>();

        static IgnoreList()
        {
            Empty = new IgnoreList(Enumerable.Empty<Regex>());
        }


        private IgnoreList(IEnumerable<Regex> seeds)
        {
            _ignoreMasks.AddRange(seeds);
        }


        public static IgnoreList Empty { get; private set; }



        public bool IsIgnored(FileInfo info)
        {
            // TODO - check subpaths?  (src/debug)
            return _ignoreMasks.Any(x => x.IsMatch(info.Name));
        }


        public bool IsIgnored(DirectoryInfo info)
        {
            return _ignoreMasks.Any(x => x.IsMatch(info.Name));
        }



        public IgnoreList Parse(FileInfo info)
        {
            IgnoreList list = new IgnoreList(_ignoreMasks);

            using (var reader = info.OpenText())
            {
                string data;
                while ((data = reader.ReadLine()) != null)
                {
                    list._ignoreMasks.Add(new Regex(data));
                }
            }

            return list;
        }
    }
}
