namespace RankedSearch
{
    using Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Iveonik.Stemmers;

    public class SearchEngine
    {
        private readonly IStemmer Stemmer;

        public SearchEngine(IStemmer stemmer)
        {
            this.Documents = new List<Document>();
            this.Stemmer = stemmer;
        }
        
        public IEnumerable<Document> Documents { get; private set; }

        public void LoadDocuments(string directoryPath)
        {
            var result = new List<Document>();

            Directory.EnumerateFiles(directoryPath).ForEach(filePath =>
            {
                result.AddRange(JsonConvert.DeserializeObject<IEnumerable<Document>>(
                    File.ReadAllText(filePath)));
            });

            this.Documents = result;
        }

        public string GetStem(string word)
        {
            return this.Stemmer.Stem(word);
        }
    }
}
