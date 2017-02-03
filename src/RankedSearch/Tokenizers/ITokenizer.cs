namespace RankedSearch.Tokenizers
{
    using System.Collections.Generic;

    public interface ITokenizer
    {
        IEnumerable<string> Tokenize(string text);

        string NormalizeToken(string token);
    }
}
