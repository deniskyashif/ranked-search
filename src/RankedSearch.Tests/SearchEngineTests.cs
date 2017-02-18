namespace RankedSearch.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RankedSearch.Stemmers;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    [TestClass]
    public class SearchEngineTests
    {
        private readonly string testCorpusPath = ConfigurationManager.AppSettings["testCorpusPath"];

        public SearchEngineTests()
        {
            if (!Directory.Exists(this.testCorpusPath))
                throw new ArgumentException("Paths for the testing data should be valid.");
        }
        
        [TestMethod]
        public void InvokingTheConstructor_WithProvidingValidArguments_ShouldCreateANewInstance()
        {
            var engine = new SearchEngine(new PorterStemmer());
            Assert.IsInstanceOfType(engine, typeof(SearchEngine));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InvokingTheConstructor_WithProvidingNullArgument_ShouldThrowAnException()
        {
            var model = new SearchEngine(null);
        }

        [TestMethod]
        public void LoadingTheDocuments_ProvidingCorrectCorpusPath_ShouldLoadAllDocuments()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.testCorpusPath);

            Assert.AreEqual(10, engine.DocumentCount);
        }
        
        [TestMethod]
        public void Searching_ForNonExistentWord_ShouldReturnNoResults()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.testCorpusPath);
            var searchResult = engine.Search("cklvxzmilaro32");

            Assert.IsFalse(searchResult.Any());
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument1()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.testCorpusPath);
            var best = engine.Search("BankAmerica Daniel Williams", 1).First();

            Assert.AreEqual("4", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument2()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.testCorpusPath);
            var best = engine.Search("BankAmerica", 1).First();

            Assert.AreEqual("4", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument3()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.testCorpusPath);
            var best = engine.Search("Texas Commerce Bancshares", 1).First();

            Assert.AreEqual("3", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument4()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.testCorpusPath);
            var best = engine.Search("COMPUTER TERMINAL SYSTEMS", 1).First();

            Assert.AreEqual("10", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument5()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.testCorpusPath);
            var best = engine.Search("stock split", 1).First();

            Assert.AreEqual("4", best.Document.Id);
        }
    }
}
