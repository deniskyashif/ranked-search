namespace Client.Console
{
    using Iveonik.Stemmers;
    using RankedSearch;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var corpusPath = @"D:\Workspace\RankedSearch\corpora\reuters-21578-json\data\justTen";
            
            var engine = new SearchEngine(new EnglishStemmer());
            engine.LoadDocuments(corpusPath);
        }
    }
}
