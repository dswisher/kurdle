using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RazorEngine.Templating;

namespace Kurdle.Misc
{


    public class EmbeddedTemplateManager : ITemplateManager
    {
        private readonly Assembly _templateAssembly;
        private readonly List<DirectoryInfo> _fileRoots = new List<DirectoryInfo>();
        private readonly string _embeddedRoot;

        public EmbeddedTemplateManager(string embeddedRoot, DirectoryInfo fileRoot)
        {
            _templateAssembly = Assembly.GetExecutingAssembly();
            _embeddedRoot = embeddedRoot;
            _fileRoots.Add(fileRoot);
        }


        public ITemplateSource Resolve(ITemplateKey key)
        {
            var embedded = key as EmbeddedTemplateKey;
            var full = key as FullPathTemplateKey;
            if ((embedded == null) && (full == null))
            {
                throw new NotSupportedException("You can only use EmbeddedTemplateKey or FullPathTemplateKey with this manager.");
            }

            if (embedded != null)
            {
                using (var stream = _templateAssembly.GetManifestResourceStream(embedded.EmbeddedPath))
                {
                    if (stream == null)
                    {
                        throw new FileNotFoundException("Could not locate template");
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        return new LoadedTemplateSource(reader.ReadToEnd());
                    }
                }
            }

            var template = File.ReadAllText(full.FullPath);
            return new LoadedTemplateSource(template, full.FullPath);
        }



        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            ITemplateKey key = GetFileKey(name, resolveType, context);

            if (key == null)
            {
                key = GetEmbeddedKey(name, resolveType, context);
            }

            if (key == null)
            {
                throw new InvalidOperationException(string.Format("Could not resolve template '{0}'", name));
            }

            return key;
        }



        private ITemplateKey GetFileKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            if (File.Exists(name))
            {
                return new FullPathTemplateKey(name, name, resolveType, context);
            }

            var resolved = _fileRoots
                .Select(l =>
                {
                    var p = Path.Combine(l.FullName, name);
                    if (File.Exists(p))
                    {
                        return p;
                    }

                    if (File.Exists(p + ".cshtml"))
                    {
                        return p + ".cshtml";
                    }

                    return null;
                })
                .FirstOrDefault(l => l != null);

            if (resolved == null)
            {
                return null;
            }

            return new FullPathTemplateKey(name, resolved, resolveType, context);
        }



        private ITemplateKey GetEmbeddedKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            string path = _embeddedRoot + "." + name;

            if (!path.EndsWith(".cshtml"))
            {
                path += ".cshtml";
            }

            var info = _templateAssembly.GetManifestResourceInfo(path);

            if (info == null)
            {
                return null;
            }

            return new EmbeddedTemplateKey(name, path, resolveType, context);
        }



        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {
            throw new NotImplementedException("dynamic templates are not supported!");
        }
    }
}
