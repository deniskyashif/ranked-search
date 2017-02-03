namespace RankedSearch.Tokenizers
{
    using Iveonik.Stemmers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
        
    public class Tokenizer: ITokenizer
    {
        private readonly IStemmer Stemmer;

        public Tokenizer(IStemmer stemmer)
        {
            this.Stemmer = stemmer;
        }
        
        public IEnumerable<string> Tokenize(string text)
        {
            return text
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(this.NormalizeToken);
        }

        public string NormalizeToken(string token)
        {
            return this.Stemmer.Stem(token.ToLower());
        }
    }
}