namespace RankedSearch
{
    using RankedSearch.LanguageModels;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Document
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Date { get; set; }

        public IEnumerable<string> Topics { get; set; }

        public IEnumerable<string> Places { get; set; }

        public ILanguageModel Model { get; set; }
    }
}
