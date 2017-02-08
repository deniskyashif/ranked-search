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

        public IEnumerable<string> Topics { get; set; } = new List<string>();
        
        public IEnumerable<string> Places { get; set; } = new List<string>();

        public ILanguageModel LanguageModel { get; set; }
    }
}
