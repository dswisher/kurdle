using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace Kurdle
{
    // Based on http://stackoverflow.com/questions/28724812/yamldotnet-deserialization-of-deeply-nested-dynamic-structures
    public static class YamlUtils
    {

        public static ExpandoObject YamlToExpando(this string yaml)
        {
            using (TextReader sr = new StringReader(yaml))
            {
                return sr.YamlToExpando();
            }
        }



        public static ExpandoObject YamlToExpando(this TextReader reader)
        {
            var stream = new YamlStream();
            stream.Load(reader);
            var firstDocument = stream.Documents[0].RootNode;
            dynamic exp = firstDocument.ToExpando();
            return exp;
        }



        public static ExpandoObject ToExpando(this YamlNode node)
        {
            ExpandoObject exp = new ExpandoObject();
            exp = (ExpandoObject)ToExpandoImpl(exp, node);
            return exp;
        }



        private static object ToExpandoImpl(ExpandoObject exp, YamlNode node)
        {
            YamlScalarNode scalar = node as YamlScalarNode;
            YamlMappingNode mapping = node as YamlMappingNode;
            YamlSequenceNode sequence = node as YamlSequenceNode;

            if (scalar != null)
            {
                string val = scalar.Value;
                return val;
            }
            else if (mapping != null)
            {
                foreach (KeyValuePair<YamlNode, YamlNode> child in mapping.Children)
                {
                    YamlScalarNode keyNode = (YamlScalarNode)child.Key;
                    string keyName = keyNode.Value;
                    object val = ToExpandoImpl(exp, child.Value);
                    var expDict = exp as IDictionary<string, object>;
                    expDict[keyName] = val;
                }
            }
            else if (sequence != null)
            {
                var childNodes = new List<object>();
                foreach (YamlNode child in sequence.Children)
                {
                    var childExp = new ExpandoObject();
                    object childVal = ToExpandoImpl(childExp, child);
                    childNodes.Add(childVal);
                }
                return childNodes;
            }

            return exp;
        }
    }
}
