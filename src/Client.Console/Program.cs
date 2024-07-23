namespace Client.Console
{
    using RankedSearch;
    using System;
    using System.Collections.Generic;
    using RankedSearch.Poco;
    using RankedSearch.Stemmers;
	using Microsoft.Extensions.Configuration;

	public class Program
    {
        private static readonly string decorationLine = string.Empty.PadLeft(100, '-');

        public static void Main()
        {
            var corpusDirectoryPath = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("corpusPath")
                .Value;
            
            Console.WriteLine("Creating the search engine...");
            var engine = new SearchEngine(new PorterStemmer());

            Console.WriteLine("Loading the documents...");
            engine.LoadDocuments(corpusDirectoryPath);
            
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

            foreach (var sr in searchResult)
            {
                Console.WriteLine($"Title: {sr.Document.Title} Id: {sr.Document.Id}");
                Console.WriteLine($"Relevance Score: {sr.RelevanceScore}");
                Console.WriteLine($"Body: {Environment.NewLine}{sr.Document.Body}");
            }
        }
    }
}
