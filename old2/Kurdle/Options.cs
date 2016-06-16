using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kurdle.Commands;


namespace Kurdle
{
    public class Options
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        private const string ConfigName = "config.yml";

        public const string SourceName = "source";
        public const string DestinationName = "destination";
        public const string VerboseName = "verbose";


        public Options(AbstractBuildCommand command)
        {
            // TODO - allow config name to be specified via the command-line
            var config = FindConfig();
            if (config == null)
            {
                throw new KurdleException("Could not find {0}.", ConfigName);
            }

            var root = config.DirectoryName;

            if (root == null)
            {
                throw new KurdleException("Could not get directory name for {0}.", config.FullName);
            }

            ReadConfig(config);

            // Override the config file with things explicitly set on the command line
            SetString(SourceName, command.SourceDir);
            SetString(DestinationName, command.DestinationDir);
            SetBool(VerboseName, command.Verbose);

            // If source and dest are not yet set, do so now.
            if (string.IsNullOrWhiteSpace(Source))
            {
                SetString(SourceName, root);
            }

            if (string.IsNullOrWhiteSpace(Destination))
            {
                SetString(DestinationName, Path.Combine(root, "output"));
            }
        }


        public string Source
        {
            get { return (string)Get(SourceName); }
        }


        public string Destination
        {
            get { return (string)Get(DestinationName); }
        }


        public bool Verbose
        {
            get
            {
                bool? val = (bool?)Get(VerboseName);

                return val.GetValueOrDefault();
            }
        }


        public object Get(string name)
        {
            return _dictionary.ContainsKey(name) ? _dictionary[name] : null;
        }


        private FileInfo FindConfig()
        {
            var dir = new DirectoryInfo(Environment.CurrentDirectory);

            while (!dir.GetFiles(ConfigName).Any())
            {
                dir = dir.Parent;
                if (dir == null)
                {
                    return null;
                }
            }

            return dir.GetFiles(ConfigName).First();
        }



        private void ReadConfig(FileInfo info)
        {
            using (var reader = info.OpenText())
            {
                var expando = reader.YamlToExpando();

                foreach (var pair in expando)
                {
                    switch (pair.Key)
                    {
                        case DestinationName:
                            if (info.DirectoryName != null)
                            {
                                var temp = new FileInfo(Path.Combine(info.DirectoryName, (string) pair.Value));
                                Set(DestinationName, temp.FullName);
                            }
                            break;

                        case SourceName:
                            if (info.DirectoryName != null)
                            {
                                var temp = new FileInfo(Path.Combine(info.DirectoryName, (string)pair.Value));
                                Set(SourceName, temp.FullName);
                            }
                            break;

                        default:
                            Set(pair.Key, pair.Value);
                            break;
                    }
                }
            }
        }



        private void Set(string name, object value)
        {
            if (_dictionary.ContainsKey(name))
            {
                _dictionary[name] = value;
            }
            else
            {
                _dictionary.Add(name, value);
            }
        }



        private void SetString(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                Set(name, value);
            }
        }


        private void SetBool(string name, bool? value)
        {
            if (value.HasValue)
            {
                Set(name, value.Value);
            }
        }
    }
}
