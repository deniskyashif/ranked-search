﻿namespace RankedSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an Unigram Language Model
    /// </summary>
    public class BagOfWords
    {
        private readonly IDictionary<string, int> termCounts;
        private int totalTermCount = 0;
        
        public BagOfWords(IEnumerable<string> text)
        {
            if (text == null)
                throw new ArgumentNullException("Text cannot be null.");

            this.termCounts = this.InferDistribution(text);
            this.totalTermCount = text.Count();
        }

        public IEnumerable<string> DistinctTerms => termCounts.Keys;

        public int GetTermCount(string term)
        {
            if (this.termCounts.ContainsKey(term))
                return this.termCounts[term];

            return 0;
        }

        public double GetTermFrequency(string term)
        {
            var termCount = this.GetTermCount(term);

            if (termCount > 0)
                return (double)termCount / totalTermCount;

            return 0;
        }
        
        private IDictionary<string, int> InferDistribution(IEnumerable<string> tokens)
        {
            var result = new Dictionary<string, int>();

            foreach (var token in tokens)
            {
                if (!result.ContainsKey(token))
                    result.Add(token, 0);
                
                result[token]++;
            }
            
            return result;
        }
    }
}
