namespace RankedSearch
{
    using Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Iveonik.Stemmers;
    using RankedSearch.Tokenizers;
    using RankedSearch.Poco;

    public class SearchEngine
    {
        private readonly ITokenizer Tokenizer;
        private IEnumerable<Document> documents;

        public SearchEngine(IStemmer stemmer)
        {
            this.Tokenizer = new Tokenizer(stemmer);
        }

        public void LoadDocuments(string directoryPath)
        {
            var result = new List<Document>();

            Directory.EnumerateFiles(directoryPath).ForEach(filePath =>
            {
                result.AddRange(JsonConvert.DeserializeObject<IEnumerable<Document>>(
                    File.ReadAllText(filePath)));
            });
        }

        public IEnumerable<SearchResult> Search(string query, int limit = 5)
        {
            throw new NotImplementedException();
        }
    }
}
