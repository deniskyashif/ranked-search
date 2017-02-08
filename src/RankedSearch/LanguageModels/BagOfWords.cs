namespace RankedSearch.LanguageModels
{
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an Unigram Language Model
    /// </summary>
    public class BagOfWords : ILanguageModel
    {
        private readonly IDictionary<string, int> termFrequencies;
        private int totalTermCount = 0;
        
        public BagOfWords(IEnumerable<string> text)
        {
            this.termFrequencies = this.InferDistribution(text);
            this.totalTermCount = text.Count();
        }

        public IEnumerable<string> Terms => termFrequencies.Keys;

        public double Query(string phrase)
        {
            if (this.termFrequencies.ContainsKey(phrase))
                return (double)this.termFrequencies[phrase] / this.totalTermCount;
            
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
