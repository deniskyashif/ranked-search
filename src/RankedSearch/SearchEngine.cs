namespace RankedSearch
{
    using Extensions;
    using Newtonsoft.Json;
    using RankedSearch.LanguageModels;
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
        private ILanguageModel corpusLanguageModel;

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
                        doc.LanguageModel = new BagOfWords(tokeinzedDocumentContent);

                        corpusText.AddRange(tokeinzedDocumentContent);

                        return doc;
                    }));
            });

            this.documents = result;
            this.corpusLanguageModel = new BagOfWords(corpusText);
        }

        public IEnumerable<SearchResult> Search(string query, int limit = 10)
        {
            if (!this.IsQueryValid(query))
                throw new ArgumentException("The provided query is invalid.");

            var queryTerms = this.TokenizeQuery(query);
            var queryLM = new BagOfWords(queryTerms);

            return this.documents
                .Select(d => new SearchResult(d, this.CalculateMaximumLikelihoodEstimate(queryTerms, d)))
                .Where(x => x.RelevanceScore > 0)
                .OrderByDescending(x => x.RelevanceScore)
                .Take(limit);
        }
        
        private double CalculateKullbackLeiblerDivergence(ILanguageModel queryLM, ILanguageModel documentLM)
        {
            var result = 0.0;

            queryLM.NGrams.ForEach(term =>
            {
                var queryLMProbability = queryLM.Query(term);
                var docLMProbability = documentLM.Query(term);

                if (docLMProbability == 0)
                    docLMProbability = corpusLanguageModel.Query(term);

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
                var score = (((1 - smoothingCoefficient) * document.LanguageModel.Query(term)) +
                    smoothingCoefficient * this.corpusLanguageModel.Query(term));

                if (score > 0)
                {
                    result *= score;
                }
                
            });

            return result;
        }

        private IEnumerable<string> TokenizeQuery(string query)
        {
            return this.tokenizer.Tokenize(query);
        }

        private bool IsQueryValid(string query)
        {
            return !string.IsNullOrWhiteSpace(query) && query.IsNormalized();
        }
    }
}
