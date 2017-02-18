namespace RankedSearch.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RankedSearch;
    using System.Linq;

    [TestClass]
    public class BagOfWordsTests
    {
        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldCreateANewInstance()
        {
            var bag = new BagOfWords(new[] { string.Empty });
            Assert.IsInstanceOfType(bag, typeof(BagOfWords));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InvokingTheConstructor_WithProvidingNullArgument_ShouldThrowAnException()
        {
            var bag = new BagOfWords(null);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution1()
        {
            var bag = new BagOfWords(new[] { "I", "run", "and", "run" });
            var actual = bag.GetTermFrequency("run");
            
            Assert.AreEqual(0.5, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution2()
        {
            var bag = new BagOfWords(new[] { "I", "run", "and", "running" });
            var actual = bag.GetTermFrequency("Run");

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution3()
        {
            var bag = new BagOfWords(new[] { "speech", "recognition", "system" });
            var actual = bag.GetTermFrequency("speech");

            Assert.AreEqual(0.33, Math.Round(actual, 2));
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution4()
        {
            var bag = new BagOfWords(new[] { "have", "having" });
            var actual = bag.GetTermFrequency("have");

            Assert.AreEqual(0.5, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution5()
        {
            var bag = new BagOfWords(new[] { "a", "b", "c", "d" });
            var actual = bag.GetTermFrequency("b");

            Assert.AreEqual(0.25, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution6()
        {
            var bag = new BagOfWords(new[] { "a", "a", "a", "aa" });
            var actual = bag.GetTermFrequency("a");

            Assert.AreEqual(0.75, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution7()
        {
            var bag = new BagOfWords(new[] { "a", "a", "a", "aa" });
            var actual = bag.GetTermFrequency("aa");

            Assert.AreEqual(0.25, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfStrings_ShouldInferCorrectDistribution8()
        {
            var bag = new BagOfWords(new[] { "hidden", "markov", "models", "hmm", "afa" });
            var actual = bag.GetTermFrequency("markov");

            Assert.AreEqual(0.2, actual);
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingAnEmptyArray_ShouldInferCorrectDistribution9()
        {
            var bag = new BagOfWords(new string[0]);
            var actual = bag.GetTermFrequency("markov");

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingAnInstance_WithProvidingAnNullArgument_ShouldThrowException()
        {
            var bag = new BagOfWords(null);
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms1()
        {
            var bag = new BagOfWords(new [] { "like", "love", "live", "like"});
            CollectionAssert.AreEqual(new[] { "like", "love", "live" }, bag.DistinctTerms.ToList());
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms2()
        {
            var bag = new BagOfWords(new [] { "like", "love", "live", "lime" });
            CollectionAssert.AreEqual(new[] { "like", "love", "live", "lime" }, bag.DistinctTerms.ToList());
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms3()
        {
            var bag = new BagOfWords(new string[] { });
            CollectionAssert.AreEqual(new string[] { }, bag.DistinctTerms.ToList());
        }

        [TestMethod]
        public void AccessingTermsProperty_ShouldReturnAllTheDistinctTerms4()
        {
            var bag = new BagOfWords(new string[] { "search", "search", "-1" });
            CollectionAssert.AreEqual(new string[] { "search", "-1" }, bag.DistinctTerms.ToList());
        }
    }
}
