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
                throw new NullReferenceException("Stemmer cannot be null.");

            this.tokenizer = new StemmingTokenizer(stemmer);
        }

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
                        var tokeinzedDocumentContent = this.tokenizer.Tokenize($"{doc.Title} {doc.Body}");
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
            var queryLM = new BagOfWords(this.TokenizeQuery(query));

            return this.documents
                .Select(d => new SearchResult(d, this.CalculateKullbackLeibrerDivergence(queryLM, d.LanguageModel)))
                .Where(x => x.RelevanceScore > 0)
                .OrderByDescending(x => x.RelevanceScore)
                .Take(limit);
        }

        private double CalculateKullbackLeibrerDivergence(ILanguageModel queryModel, ILanguageModel documentModel)
        {
            double result = 0;

            foreach (var term in documentModel.Terms)
            {
                var queryLMProbability = queryModel.Query(term);
                var docLMProbability = documentModel.Query(term);

                if (queryLMProbability > 0)
                    result += (queryLMProbability * Math.Log(queryLMProbability / docLMProbability));
            }

            return result;
        }

        private double CalculateMaximumLikelihoodScore(IEnumerable<string> queryTerms, Document document)
        {
            double smoothingCoefficient = 0.5;
            double result = 1;

            foreach (var term in queryTerms)
            {
                result *= (((1 - smoothingCoefficient) * document.LanguageModel.Query(term)) +
                    smoothingCoefficient * this.corpusLanguageModel.Query(term));
            }

            return result;
        }

        private IEnumerable<string> TokenizeQuery(string query)
        {
            return query.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(tokenizer.NormalizeToken);
        }
    }
}
