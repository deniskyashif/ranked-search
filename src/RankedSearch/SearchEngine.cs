namespace RankedSearch
{
    using Extensions;
    using Newtonsoft.Json;
    using RankedSearch.Poco;
    using RankedSearch.Stemmers;
    using RankedSearch.Tokenizers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class SearchEngine
    {
        private const int MinDocBodyLength = 1;

        private readonly ITokenizer tokenizer;
        private IEnumerable<Document> documents;
        private BagOfWords corpusBagOfWords;

        public SearchEngine(IStemmer stemmer)
        {
            if (stemmer == null)
                throw new ArgumentNullException("A valid stemmer should be provided.");

            this.tokenizer = new StemmingTokenizer(stemmer);
        }

        public int DocumentCount => this.documents.Count();

        public void LoadDocuments(string directoryPath)
        {
            var result = new List<Document>();
            var corpusText = new List<string>();

            Directory.EnumerateFiles(directoryPath).ForEach(filePath =>
            {
                result.AddRange(
                    JsonConvert.DeserializeObject<IEnumerable<Document>>(File.ReadAllText(filePath))
                    .Where(doc => doc.Body != null && doc.Body.Length > MinDocBodyLength)
                    .Select(doc =>
                    {
                        var content = $"{doc.Title} {doc.Body} ";
                        var tokeinzedDocumentContent = this.tokenizer.Tokenize(content);

                        doc.BagOfWords = new BagOfWords(tokeinzedDocumentContent);
                        corpusText.AddRange(tokeinzedDocumentContent);

                        return doc;
                    }));
            });

            this.corpusBagOfWords = new BagOfWords(corpusText);
            this.documents = result;
        }

        public IEnumerable<SearchResult> Search(string query, int limit = 10)
        {
            if (!this.IsQueryValid(query))
                throw new ArgumentException("The provided query is invalid.");

            var queryTerms = this.TokenizeQuery(query);
            var queryLM = new BagOfWords(queryTerms);

            return this.documents
                .Select(d => new SearchResult(d, this.CalculateKullbackLeiblerDivergence(queryLM, d.BagOfWords)))
                .Where(x => x.RelevanceScore > 0)
                .OrderBy(x => x.RelevanceScore)
                .Take(limit);
        }

        private IEnumerable<string> TokenizeQuery(string query)
        {
            return this.tokenizer.Tokenize(query);
        }

        private bool IsQueryValid(string query)
        {
            return !string.IsNullOrWhiteSpace(query) && query.IsNormalized();
        }

        private double CalculateKullbackLeiblerDivergence(BagOfWords queryLM, BagOfWords documentLM)
        {
            var result = 0.0;

            queryLM.DistinctTerms.ForEach(term =>
            {
                var queryLMProbability = queryLM.GetTermFrequency(term);
                var docLMProbability = documentLM.GetTermFrequency(term);

                if (docLMProbability == 0)
                    docLMProbability = corpusBagOfWords.GetTermFrequency(term);

                if (docLMProbability > 0)
                    result += (queryLMProbability * Math.Log(queryLMProbability / docLMProbability));
            });

            return result;
        }

        private double CalculateMaximumLikelihoodEstimate(IEnumerable<string> queryTerms, Document document)
        {
            var smoothingCoefficient = 0.5;
            var result = 1.0;

            queryTerms.ForEach(term =>
            {
                var score = (((1 - smoothingCoefficient) * document.BagOfWords.GetTermFrequency(term)) +
                    smoothingCoefficient * this.corpusBagOfWords.GetTermFrequency(term));

                if (score > 0)
                {
                    result *= score;
                }
                
            });

            return result;
        }
    }
}
