namespace RankedSearch.Tests
{
    using Iveonik.Stemmers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class SearchEngineTests
    {
        private IMock<IStemmer> stemmerMock = new Mock<IStemmer>();

        [TestMethod]
        public void InvokingTheConstructor_WithProvidingValidArguments_ShouldCreateANewInstance()
        {
            var model = new SearchEngine(this.stemmerMock.Object);
            Assert.IsInstanceOfType(model, typeof(SearchEngine));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void InvokingTheConstructor_WithProvidingNullArgument_ShouldThrowAnException()
        {
            var model = new SearchEngine(null);
        }
    }
}
