namespace Client.Console
{
    using Iveonik.Stemmers;
    using RankedSearch;
    using System;
    using System.Configuration;
    using Extensions;
    using System.Collections.Generic;
    using RankedSearch.Poco;

    public class Program
    {
        private static readonly string decorationLine = string.Empty.PadLeft(100, '-');

        public static void Main()
        {
            var corpusDirectory = ConfigurationManager.AppSettings["corpusPath"];

            Console.WriteLine("Creating the search engine...");
            var engine = new SearchEngine(new EnglishStemmer());

            Console.WriteLine("Loading the documents...");
            engine.LoadDocuments(corpusDirectory);
            
            while (true)
            {
                Console.Write(">_ ");

                var query = Console.ReadLine();
                
                try
                {
                    var searchResults = engine.Search(query, 3);
                    PrintResults(query, searchResults);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void PrintResults(string query, IEnumerable<SearchResult> searchResult)
        {
            Console.WriteLine(decorationLine);
            Console.WriteLine($"Results for query \"{query}\":");
            Console.WriteLine(decorationLine);

            searchResult.ForEach(sr =>
            {
                Console.WriteLine($"Title: {sr.Document.Title} Id: {sr.Document.Id}");
                Console.WriteLine($"Relevance Score: {sr.RelevanceScore}");
                Console.WriteLine($"Body: {Environment.NewLine}{sr.Document.Body}");
            });
        }
    }
}
