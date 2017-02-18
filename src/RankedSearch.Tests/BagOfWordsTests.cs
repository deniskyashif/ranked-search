namespace RankedSearch.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RankedSearch.LanguageModels;
    using System.Linq;

    [TestClass]
    public class BagOfWordsTests
    {
        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldCreateANewInstance()
        {
            var model = new BagOfWords(new[] { string.Empty });
            Assert.IsInstanceOfType(model, typeof(BagOfWords));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InvokingTheConstructor_WithProvidingNullArgument_ShouldThrowAnException()
        {
            var model = new BagOfWords(null);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution1()
        {
            var model = new BagOfWords(new[] { "I", "run", "and", "run" });
            var actual = model.Query("run");
            
            Assert.AreEqual(0.5, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution2()
        {
            var model = new BagOfWords(new[] { "I", "run", "and", "running" });
            var actual = model.Query("Run");

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution3()
        {
            var model = new BagOfWords(new[] { "speech", "recognition", "system" });
            var actual = model.Query("speech");

            Assert.AreEqual(0.33, Math.Round(actual, 2));
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution4()
        {
            var model = new BagOfWords(new[] { "have", "having" });
            var actual = model.Query("have");

            Assert.AreEqual(0.5, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution5()
        {
            var model = new BagOfWords(new[] { "a", "b", "c", "d" });
            var actual = model.Query("b");

            Assert.AreEqual(0.25, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution6()
        {
            var model = new BagOfWords(new[] { "a", "a", "a", "aa" });
            var actual = model.Query("a");

            Assert.AreEqual(0.75, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution7()
        {
            var model = new BagOfWords(new[] { "a", "a", "a", "aa" });
            var actual = model.Query("aa");

            Assert.AreEqual(0.25, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution8()
        {
            var model = new BagOfWords(new[] { "hidden", "markov", "models", "hmm", "afa" });
            var actual = model.Query("markov");

            Assert.AreEqual(0.2, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingAnEmptyArray_ShouldInferCorrectDistribution9()
        {
            var model = new BagOfWords(new string[0]);
            var actual = model.Query("markov");

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingAnInstance_WithProvidingAnNullArgument_ShouldThrowException()
        {
            var model = new BagOfWords(null);
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms1()
        {
            var model = new BagOfWords(new [] { "like", "love", "live", "like"});
            CollectionAssert.AreEqual(new[] { "like", "love", "live" }, model.NGrams.ToList());
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms2()
        {
            var model = new BagOfWords(new [] { "like", "love", "live", "lime" });
            CollectionAssert.AreEqual(new[] { "like", "love", "live", "lime" }, model.NGrams.ToList());
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms3()
        {
            var model = new BagOfWords(new string[] { });
            CollectionAssert.AreEqual(new string[] { }, model.NGrams.ToList());
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms4()
        {
            var model = new BagOfWords(new string[] { "search", "search", "-1" });
            CollectionAssert.AreEqual(new string[] { "search", "-1" }, model.NGrams.ToList());
        }
    }
}
