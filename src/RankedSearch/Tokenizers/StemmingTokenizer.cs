namespace RankedSearch.Tokenizers
{
    using Iveonik.Stemmers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
        
    public class StemmingTokenizer: ITokenizer
    {
        private readonly IStemmer Stemmer;

        public StemmingTokenizer(IStemmer stemmer)
        {
            this.Stemmer = stemmer;
        }
        
        public IEnumerable<string> Tokenize(string text)
        {
            return text
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(this.NormalizeToken);
        }

        /// <summary>
        /// Infers the least weighted grammatical form of a token(word)
        /// </summary>
        /// <param name="token">The input token as string</param>
        /// <returns>The normalized token as string</returns>
        public string NormalizeToken(string token)
        {
            return this.Stemmer.Stem(token.ToLower());
        }
    }
}