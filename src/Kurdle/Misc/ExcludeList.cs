using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Kurdle.Misc
{
    public class ExcludeList
    {
        private readonly List<Regex> _excludeMasks = new List<Regex>();

        static ExcludeList()
        {
            Empty = new ExcludeList(Enumerable.Empty<Regex>());
        }


        private ExcludeList(IEnumerable<Regex> seeds)
        {
            _excludeMasks.AddRange(seeds);
        }


        public static ExcludeList Empty { get; private set; }



        public bool IsIgnored(FileInfo info)
        {
            return _excludeMasks.Any(x => x.IsMatch(info.Name));
        }


        public bool IsIgnored(DirectoryInfo info)
        {
            return _excludeMasks.Any(x => x.IsMatch(info.Name));
        }


        public ExcludeList Parse(FileInfo info)
        {
            ExcludeList list = new ExcludeList(_excludeMasks);

            using (var reader = info.OpenText())
            {
                string data;
                while ((data = reader.ReadLine()) != null)
                {
                    list._excludeMasks.Add(new Regex(data));
                }
            }

            return list;
        }
    }
}
