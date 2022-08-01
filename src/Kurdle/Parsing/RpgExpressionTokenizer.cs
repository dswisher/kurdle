using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Kurdle.Parsing
{
    public static class RpgExpressionTokenizer
    {
        public static Tokenizer<RpgExpressionToken> Instance { get; } =
            new TokenizerBuilder<RpgExpressionToken>()
                .Ignore(Span.WhiteSpace)
                .Match(Character.EqualTo('='), RpgExpressionToken.Equals)
                .Match(Character.EqualTo('\n'), RpgExpressionToken.NewLine)
                .Match(Numerics.Natural, RpgExpressionToken.Number)
                .Match(Identifier.CStyle, RpgExpressionToken.Identifier)
                .Build();
    }
}
