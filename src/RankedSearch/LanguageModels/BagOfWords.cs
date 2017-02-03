namespace RankedSearch.LanguageModels
{
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an Unigram Language Model
    /// </summary>
    public class BagOfWords
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
