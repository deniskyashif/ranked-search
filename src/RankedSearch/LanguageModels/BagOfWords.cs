namespace RankedSearch.LanguageModels
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an Unigram Language Model
    /// </summary>
    public class BagOfWords : ILanguageModel
    {
        private readonly IDictionary<string, int> unigramFrequencies;
        private int totalCount = 0;
        
        public BagOfWords(IEnumerable<string> text)
        {
            if (text == null)
                throw new NullReferenceException("Text cannot be null.");

            this.unigramFrequencies = this.InferDistribution(text);
            this.totalCount = text.Count();
        }

        public IEnumerable<string> NGrams => unigramFrequencies.Keys;

        public double Query(string phrase)
        {
            var words = phrase.Split(' ');
            var result = this.GetProbability(words.First());

            words.Skip(1)?.ForEach(w =>
            {
                result *= this.GetProbability(w);
            });
            
            return result;
        }

        private double GetProbability(string word)
        {
            if (this.unigramFrequencies.ContainsKey(word))
                return ((double)this.unigramFrequencies[word] / this.totalCount);

            return 0;
        }
        
        private IDictionary<string, int> InferDistribution(IEnumerable<string> tokens)
        {
            var result = new Dictionary<string, int>();

            tokens.ForEach(token =>
            {
                if (!result.ContainsKey(token))
                    result.Add(token, 0);
                
                result[token]++;
            });
            
            return result;
        }
    }
}
