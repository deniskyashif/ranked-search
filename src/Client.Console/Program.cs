namespace Client.Console
{
    using Iveonik.Stemmers;
    using RankedSearch;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var engine = new SearchEngine(new EnglishStemmer());
            engine.LoadDocuments(@"D:\Workspace\RankedSearch\data\justTen");
        }
    }
}
