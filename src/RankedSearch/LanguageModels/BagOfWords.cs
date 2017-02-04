﻿namespace RankedSearch.LanguageModels
{
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an Unigram Language Model
    /// </summary>
    public class BagOfWords: ILanguageModel
    {
        private readonly IDictionary<string, double> Distribution;
        
        public BagOfWords(IEnumerable<string> text)
        {
            this.Distribution = this.InferDistribution(text);
        }

        public double Query(string phrase)
        {
            if (this.Distribution.ContainsKey(phrase))
                return this.Distribution[phrase];
            
            return 0;
        }
        
        private IDictionary<string, double> InferDistribution(IEnumerable<string> tokens)
        {
            var counts = new Dictionary<string, double>();

            tokens.ForEach(token =>
            {
                if (!counts.ContainsKey(token))
                    counts.Add(token, 0);
                
                counts[token]++;
            });

            var totalCount = tokens.Count();
            var distribution = new Dictionary<string, double>();

            counts.ForEach(pair => distribution.Add(pair.Key, pair.Value / totalCount));

            return distribution;
        }
    }
}
