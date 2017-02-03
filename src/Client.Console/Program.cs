namespace Client.Console
{
    using Iveonik.Stemmers;
    using RankedSearch;
    using System;
    using System.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var corpusDirectory = ConfigurationManager.AppSettings["corpusDirectory"];
            
            var engine = new SearchEngine(new EnglishStemmer());
            engine.LoadDocuments(corpusDirectory);
        }
    }
}
