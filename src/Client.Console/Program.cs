namespace Client.Console
{
    using Iveonik.Stemmers;
    using RankedSearch;
    using System;
    using System.Configuration;
    using Extensions;

    public class Program
    {
        private static readonly string decorationLine = string.Empty.PadLeft(40, '-');

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
                
                var input = Console.ReadLine();
                var result = engine.Search(input, 1);

                Console.WriteLine(decorationLine);
                Console.WriteLine($"Results for query \"{input}\":");
                Console.WriteLine(decorationLine);

                result.ForEach(doc =>
                {
                    Console.WriteLine(doc.Document.Title);
                    Console.WriteLine($"Relevance Score: {doc.RelevanceScore}");
                    Console.WriteLine($"Body: {Environment.NewLine}{doc.Document.Body}");
                });
            }
        }
    }
}
