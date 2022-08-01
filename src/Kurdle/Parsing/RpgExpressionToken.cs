using System.ComponentModel;
using Superpower.Display;

namespace Kurdle.Parsing
{
    public enum RpgExpressionToken
    {
        None,

        Number,

        [Token(Category = "separator")]
        NewLine,

        [Token(Category = "operator", Example = "=")]
        Equals,

        Identifier
    }
}
