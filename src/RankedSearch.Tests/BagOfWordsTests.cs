namespace RankedSearch.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RankedSearch.LanguageModels;

    [TestClass]
    public class BagOfWordsTests
    {
        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldCreateANewInstance()
        {
            var model = new BagOfWords(new[] { "" });
            Assert.IsInstanceOfType(model, typeof(BagOfWords));
        }

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldInferCorrectDistribution()
        {
            var tokens = new[] { "I", "run", "and", "run" };
            var model = new BagOfWords(tokens);
            var actual = model.Query("run");
            
            Assert.AreEqual(0.5, actual);
        }

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldInferCorrectDistribution1()
        {
            var tokens = new[] { "I", "run", "and", "run" };
            var model = new BagOfWords(tokens);
            var actual = model.Query("Run");

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void InvokingTheConstructor_WithEnumerableOfStrings_ShouldInferCorrectDistribution2()
        {
            var tokens = new[] { "speech", "recognition", "system" };
            var model = new BagOfWords(tokens);
            var actual = model.Query("speech");

            Assert.AreEqual(0.33, Math.Round(actual, 2));
        }
    }
}
