namespace RankedSearch.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RankedSearch.LanguageModels;
    using Iveonik.Stemmers;
    using RankedSearch.Tokenizers;

    [TestClass]
    public class BagOfWordsTests
    {
        private ITokenizer tokenizer = new Tokenizer(new EnglishStemmer());

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldCreateANewInstance()
        {
            var model = new BagOfWords(string.Empty, tokenizer);
            Assert.IsInstanceOfType(model, typeof(BagOfWords));
        }

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldInferCorrectDistribution()
        {
            var model = new BagOfWords("I run and run", tokenizer);
            var actual = model.Query("run");
            
            Assert.AreEqual(0.5, actual);
        }

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldInferCorrectDistribution1()
        {
            var model = new BagOfWords("I run and running", tokenizer);
            var actual = model.Query("Run");

            Assert.AreEqual(0.5, actual);
        }

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldInferCorrectDistribution2()
        {
            var model = new BagOfWords("speech recognition system", tokenizer);
            var actual = model.Query("speech");

            Assert.AreEqual(0.33, Math.Round(actual, 2));
        }

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldInferCorrectDistribution3()
        {
            var model = new BagOfWords("have having", tokenizer);
            var actual = model.Query("have");

            Assert.AreEqual(1, actual);
        }
    }
}
