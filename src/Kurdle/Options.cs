using System.Collections.Generic;
using Kurdle.Commands;

namespace Kurdle
{
    public class Options
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        private const string SourceName = "source";
        private const string DestinationName = "destination";


        public Options(AbstractBuildCommand command)
        {
            // TODO - look for config file and parse it

            // Override the config file with things explicitly set on the command line
            SetString(SourceName, command.SourceDir);
            SetString(DestinationName, command.DestinationDir);

            // TODO - if source and dest do not have values, apply defaults
        }


        public string Source
        {
            get { return GetString(SourceName); }
        }


        public string Destination
        {
            get { return GetString(DestinationName); }
        }


        private string GetString(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name];
            }

            return null;
        }


        private void SetString(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
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
        }
    }
}
