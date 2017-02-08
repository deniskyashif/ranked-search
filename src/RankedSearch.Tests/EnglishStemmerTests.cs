namespace RankedSearch.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Iveonik.Stemmers;
    using System;

    [TestClass]
    public class EnglishStemmerTests
    {
        private IStemmer stemmer = new EnglishStemmer();

        [TestMethod]
        public void InvokingTheConstructor_WithNoArguments_ShouldReturnNewInstance()
        {
            var stemmer = new EnglishStemmer();
            Assert.IsInstanceOfType(stemmer, typeof(EnglishStemmer));
        }

        [TestMethod]
        public void Stemming_TheWordReading_ShouldReturnRead()
        {
            var actual = this.stemmer.Stem("reading");
            Assert.AreEqual("read", actual);
        }

        [TestMethod]
        public void Stemming_TheWordCriticized_ShouldReturnCritic()
        {
            var actual = this.stemmer.Stem("criticized");
            Assert.AreEqual("critic", actual);
        }

        [TestMethod]
        public void Stemming_TheWordMoisturize_ShouldReturnMoistur()
        {
            var stemmer = new EnglishStemmer();
            var actual = this.stemmer.Stem("moisturize");
            Assert.AreEqual("moistur", actual);
        }

        [TestMethod]
        public void Stemming_TheWord1234_ShouldReturn1234()
        {
            var actual = this.stemmer.Stem("1234");
            Assert.AreEqual("1234", actual);
        }

        [TestMethod]
        public void Stemming_ADateString_ShouldReturnTheSame()
        {
            var expected = DateTime.Now.ToShortDateString();
            var actual = this.stemmer.Stem(expected);
            Assert.AreEqual(expected, actual);
        }
    }
}
