namespace RankedSearch.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Iveonik.Stemmers;
    
    [TestClass]
    public class EnglishStemmerTests
    {
        [TestMethod]
        public void InvokingTheConstructor_WithNoArguments_ShouldReturnNewInstance()
        {
            var stemmer = new EnglishStemmer();
            Assert.IsInstanceOfType(stemmer, typeof(EnglishStemmer));
        }

        [TestMethod]
        public void Stemming_TheWordReading_ShouldReturnRead()
        {
            var stemmer = new EnglishStemmer();
            var actual = stemmer.Stem("reading");
            Assert.AreEqual("read", actual);
        }

        [TestMethod]
        public void Stemming_TheWordCriticized_ShouldReturnCritic()
        {
            var stemmer = new EnglishStemmer();
            var actual = stemmer.Stem("criticized");
            Assert.AreEqual("critic", actual);
        }

        [TestMethod]
        public void Stemming_TheWordMoisturize_ShouldReturnMoistur()
        {
            var stemmer = new EnglishStemmer();
            var actual = stemmer.Stem("moisturize");
            Assert.AreEqual("moistur", actual);
        }
    }
}
