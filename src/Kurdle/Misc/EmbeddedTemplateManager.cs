﻿using System;
using System.IO;
using System.Reflection;
using RazorEngine.Templating;

namespace Kurdle.Misc
{


    public class EmbeddedTemplateManager : ITemplateManager
    {
        private readonly Assembly _templateAssembly;
        private readonly string _root;

        public EmbeddedTemplateManager(string root)
        {
            _templateAssembly = Assembly.GetExecutingAssembly();
            _root = root;
        }


        public ITemplateSource Resolve(ITemplateKey key)
        {
            var embedded = key as EmbeddedTemplateKey;
            if (embedded == null)
            {
                throw new NotSupportedException("You can only use EmbeddedTemplateKey with this manager.");
            }

            using (var stream = _templateAssembly.GetManifestResourceStream (embedded.EmbeddedPath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Could not locate template");
                }

                using (var reader = new StreamReader (stream))
                {
                    return new LoadedTemplateSource(reader.ReadToEnd());
                }
            }
        }


        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            string path = _root + "." + name;

            if (!path.EndsWith(".cshtml"))
            {
                path += ".cshtml";
            }

            var info = _templateAssembly.GetManifestResourceInfo(path);

            if (info == null)
            {
                throw new InvalidOperationException(string.Format("Could not resolve template '{0}'", name));
            }

            return new EmbeddedTemplateKey(name, path, resolveType, context);
        }


        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {
            throw new NotImplementedException("dynamic templates are not supported!");
        }
    }
}