using System.Linq;
using FluentAssertions;
using Kurdle.Parsing;
using Xunit;

namespace Kurdle.Tests.Parsing
{
    public class RpgExpressionTokenizerTests
    {
        [Theory]
        [InlineData("foo = 1", RpgExpressionToken.Identifier, RpgExpressionToken.Equals, RpgExpressionToken.Number)]
        public void CanTokenize(string input, params RpgExpressionToken[] expectedTokens)
        {
            // Act
            var actualTokens = RpgExpressionTokenizer.Instance.Tokenize(input).ToList();

            // Assert
            actualTokens.Should().HaveCount(expectedTokens.Length);

            for (int i = 0; i < expectedTokens.Length; i++)
            {
                actualTokens[i].Kind.Should().Be(expectedTokens[i]);
            }
        }
    }
}
