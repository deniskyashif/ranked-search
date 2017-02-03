namespace RankedSearch.LanguageModels
{
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an Unigram Language Model
    /// </summary>
    internal class BagOfWords
    {
        private readonly IDictionary<string, double> Distribution;

        public BagOfWords(IEnumerable<string> tokens)
        {
            this.Distribution = this.InferDistribution(tokens);
        }

        public double Query(string text)
        {
            if (this.Distribution.ContainsKey(text))
            {
                return this.Distribution[text];
            }

            return 0;
        }

        private IDictionary<string, double> InferDistribution(IEnumerable<string> tokens)
        {
            var distribution = new Dictionary<string, double>();

            tokens.ForEach(token =>
            {
                if (!distribution.ContainsKey(token))
                    distribution.Add(token, 1);
                
                distribution[token]++;
            });

            var tokenCount = tokens.Count();
            distribution.ForEach(pair => distribution[pair.Key] = pair.Value / tokenCount);

            return distribution;
        }
    }
}
