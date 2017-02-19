namespace RankedSearch
{
    using Extensions;
    using Newtonsoft.Json;
    using RankedSearch.Poco;
    using RankedSearch.Stemmers;
    using RankedSearch.Tokenizers;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class SearchEngine
    {
        private const int MinDocBodyLength = 1;

        private readonly ITokenizer tokenizer;
        private IEnumerable<Document> documents;
        private InvertedIndex invertedIndex;
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
            var documents = new List<Document>();
            var corpusText = new List<string>();

            Directory.EnumerateFiles(directoryPath).ForEach(filePath =>
            {
                documents.AddRange(
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

            this.documents = documents;
            this.corpusBagOfWords = new BagOfWords(corpusText);
            this.invertedIndex = new InvertedIndex(documents);
        }

        public IEnumerable<SearchResult> Search(string query, int limit = 10)
        {
            if (!this.IsQueryValid(query))
                throw new ArgumentException("The provided query is invalid.");

            var queryTerms = this.TokenizeQuery(query);
            var queryLM = new BagOfWords(queryTerms);
            var relevantDocuments = this.invertedIndex.GetDocumentsContainingTerms(queryTerms);
            
            return relevantDocuments
                .Select(doc => new SearchResult(doc, this.CalculateKullbackLeiblerDivergence(queryLM, doc.BagOfWords)))
                .OrderByDescending(sr => sr.RelevanceScore)
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
                    docLMProbability = this.corpusBagOfWords.GetTermFrequency(term);

                if (docLMProbability > 0)
                    result += (queryLMProbability * Math.Log(queryLMProbability / docLMProbability));
            });

            return result;
        }

        private double CalculateTfIdfRelevanceScore(IEnumerable<string> query, Document doc)
        {
            return query.Aggregate(0.0, (sum, term) => sum += this.CalculateTfIdf(term, doc));
        }

        private double CalculateTfIdf(string term, Document doc)
        {
            var tf = doc.BagOfWords.GetTermFrequency(term);
            var idf = this.invertedIndex.GetInverseDocumentFrequency(term);

            return tf * idf;
        }
    }
}
