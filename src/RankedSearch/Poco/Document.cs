namespace RankedSearch
{
    using RankedSearch.LanguageModels;
    using System.Collections.Generic;

    public class Document
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Date { get; set; }

        public IEnumerable<string> Topics { get; set; }

        public IEnumerable<string> Places { get; set; }

        public ILanguageModel LanguageModel { get; set; }
    }
}
