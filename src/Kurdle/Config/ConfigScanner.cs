using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kurdle.Config.Models;
using Kurdle.DepGraph.Models;
using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kurdle.Config
{
    public class ConfigScanner : IConfigScanner
    {
        private readonly IScopeUpdater scopeUpdater;
        private readonly ILogger logger;

        public ConfigScanner(IScopeUpdater scopeUpdater, ILogger logger)
        {
            this.scopeUpdater = scopeUpdater;
            this.logger = logger;
        }


        public async Task ScanAsync(ScanScope scope, HashSet<FileInfo> files, CancellationToken cancellationToken)
        {
            // TODO - handle various flavors/names - kurdle.yaml, kurdle.json, config.yaml, etc.
            // TODO - for now, just handle kurdle.yaml
            var configFileInfo = files.FirstOrDefault(x => x.Name == "kurdle.yaml");

            if (configFileInfo == null)
            {
                return;
            }

            files.Remove(configFileInfo);

            // Load the yaml
            ScopeConfig config;

            using (var stream = new FileStream(configFileInfo.FullName, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                config = deserializer.Deserialize<ScopeConfig>(reader);
            }

            // TODO
            await Task.CompletedTask;   // TODO - other config types hopefully have async loading available
            var foo = cancellationToken;

            // Apply the config to the scope
            scopeUpdater.Update(scope, config);
        }
    }
}
