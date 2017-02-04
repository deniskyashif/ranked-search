namespace Client.Console
{
    using Iveonik.Stemmers;
    using RankedSearch;
    using System;
    using System.Configuration;

    public class Program
    {
        public static void Main()
        {
            var corpusDirectory = ConfigurationManager.AppSettings["corpusDirectory"];

            Console.WriteLine("Creating the search engine...");
            var engine = new SearchEngine(new EnglishStemmer());

            Console.WriteLine("Loading the documents...");
            engine.LoadDocuments(corpusDirectory);
        }
    }
}
