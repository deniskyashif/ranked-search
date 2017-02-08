namespace RankedSearch
{
    using Extensions;
    using Iveonik.Stemmers;
    using Newtonsoft.Json;
    using RankedSearch.LanguageModels;
    using RankedSearch.Poco;
    using RankedSearch.Tokenizers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class SearchEngine
    {
        private readonly ITokenizer tokenizer;
        private IEnumerable<Document> documents;
        private ILanguageModel corpusLanguageModel;

        public SearchEngine(IStemmer stemmer)
        {
            if (stemmer == null)
                throw new NullReferenceException("A valid stemmer should be provided.");

            this.tokenizer = new StemmingTokenizer(stemmer);
        }

        public int DocumentsCount => this.documents.Count();

        public void LoadDocuments(string directoryPath)
        {
            var result = new List<Document>();
            var corpusText = new List<string>();

            Directory.EnumerateFiles(directoryPath).ForEach(filePath =>
            {
                result.AddRange(
                    JsonConvert.DeserializeObject<IEnumerable<Document>>(File.ReadAllText(filePath))
                    .Where(doc => doc.Body != null && doc.Body.Length > 0)
                    .Select(doc =>
                    {
                        var content = $"{doc.Title} {doc.Body} " +
                            $"{string.Join(" ", doc.Places)} " +
                            $"{string.Join(" ", doc.Topics)}";

                        var tokeinzedDocumentContent = this.tokenizer.Tokenize(content);
                        doc.LanguageModel = new BagOfWords(tokeinzedDocumentContent);

                        corpusText.AddRange(tokeinzedDocumentContent);

                        return doc;
                    }));
            });

            this.documents = result;
            this.corpusLanguageModel = new BagOfWords(corpusText);
        }

        public IEnumerable<SearchResult> Search(string query, int limit = 5)
        {
            if (!this.IsQueryValid(query))
                throw new ArgumentException("The provided query is invalid.");

            var queryLM = new BagOfWords(this.TokenizeQuery(query));

            return this.documents
                .Select(d => new SearchResult(d, this.CalculateKullbackLeibrerDivergence(queryLM, d.LanguageModel)))
                .Where(x => x.RelevanceScore > 0)
                .OrderByDescending(x => x.RelevanceScore)
                .Take(limit);
        }

        private double CalculateKullbackLeibrerDivergence(ILanguageModel queryLM, ILanguageModel documentLM)
        {
            var result = 0.0;

            documentLM.NGrams.ForEach(term =>
            {
                var queryLMProbability = queryLM.Query(term);
                var docLMProbability = documentLM.Query(term);

                if (queryLMProbability > 0)
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
                result *= (((1 - smoothingCoefficient) * document.LanguageModel.Query(term)) +
                    smoothingCoefficient * this.corpusLanguageModel.Query(term));
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
