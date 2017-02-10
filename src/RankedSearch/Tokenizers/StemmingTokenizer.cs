namespace RankedSearch.Tokenizers
{
    using RankedSearch.Stemmers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
        
    public class StemmingTokenizer : ITokenizer
    {
        private readonly IStemmer stemmer;
        private readonly string[] separators = new string[] 
            { " ", "\t", ",", ".", "?", "!", ":", "/", "-", "(", ")", "<", ">", "+", "'", Environment.NewLine };
        
        public StemmingTokenizer(IStemmer stemmer)
        {
            this.stemmer = stemmer;
        }
        
        public IEnumerable<string> Tokenize(string text)
        {
            return text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(this.NormalizeToken);
        }

        /// <summary>
        /// Infers the least weighted grammatical form of a token(word)
        /// </summary>
        /// <param name="token">The input token as string</param>
        /// <returns>The normalized token as string</returns>
        public string NormalizeToken(string token)
        {
            return this.stemmer.Stem(token.ToLower());
        }
    }
}