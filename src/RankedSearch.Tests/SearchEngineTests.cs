namespace RankedSearch.Tests
{
	using Microsoft.Extensions.Configuration;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RankedSearch.Stemmers;
    using System;
    using System.IO;
    using System.Linq;

    [TestClass]
    public class SearchEngineTests
    {
        private readonly string corpusDirectory;

        public SearchEngineTests()
        {
			this.corpusDirectory = new ConfigurationBuilder()
	            .AddJsonFile("appsettings.json")
	            .Build()
	            .GetSection("corpusPath")
	            .Value;

			if (!Directory.Exists(corpusDirectory))
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
            engine.LoadDocuments(this.corpusDirectory);

            Assert.AreEqual(10, engine.DocumentCount);
        }
        
        [TestMethod]
        public void Searching_ForUnseenWord_ShouldReturnNoResults()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.corpusDirectory);
            var searchResult = engine.Search("cklvxzmilaro32");

            Assert.IsFalse(searchResult.Any());
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument1()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.corpusDirectory);
            var best = engine.Search("BankAmerica Daniel Williams", 1).First();

            Assert.AreEqual("4", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument2()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.corpusDirectory);
            var best = engine.Search("BankAmerica", 1).First();

            Assert.AreEqual("4", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument3()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.corpusDirectory);
            var best = engine.Search("Texas Commerce Bancshares", 1).First();

            Assert.AreEqual("3", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument4()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.corpusDirectory);
            var best = engine.Search("COMPUTER TERMINAL SYSTEMS", 1).First();

            Assert.AreEqual("10", best.Document.Id);
        }

        [TestMethod]
        public void Searching_WithValidQuery_ShouldReturnTheMostRelevantDocument5()
        {
            var engine = new SearchEngine(new PorterStemmer());
            engine.LoadDocuments(this.corpusDirectory);
            var best = engine.Search("Merrill Lynch", 1).First();
            
            Assert.AreEqual("4", best.Document.Id);
        }
    }
}
