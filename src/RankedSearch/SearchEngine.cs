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
    using System.Text;

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
                        var tokeinzedDocumentBody = this.tokenizer.Tokenize(doc.Body);
                        doc.Model = new BagOfWords(tokeinzedDocumentBody);
                        corpusText.AddRange(tokeinzedDocumentBody);

                        return doc;
                    }));
            });

            this.documents = result;
            this.corpusLanguageModel = new BagOfWords(corpusText);
        }

        public SearchResult Search(string query, int limit = 5)
        {
            var queryTerms = this.ParseQuery(query);
            
            var firstDocument = this.documents.First();
            var bestMatch = new SearchResult(firstDocument, this.CalculateRelevanceScore(queryTerms, firstDocument));
            
            foreach (var document in this.documents.Skip(1))
            {
                var relevanceScore = this.CalculateRelevanceScore(queryTerms, document);

                if (relevanceScore > bestMatch.RelevanceScore)
                    bestMatch = new SearchResult(document, relevanceScore);
            }

            return bestMatch;
        }

        private double CalculateRelevanceScore(IEnumerable<string> queryTerms, Document document)
        {
            double smoothingCoefficient = 0.5;

            var similariy = queryTerms.Aggregate<string, double>(1, (product, term) =>
            {
                product = ((1 - smoothingCoefficient) * document.Model.Query(term)) +
                    smoothingCoefficient * corpusLanguageModel.Query(term);

                return product;
            });

            return similariy;
        }

        private IEnumerable<string> ParseQuery(string query)
        {
            return query.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(tokenizer.NormalizeToken);
        }
    }
}
