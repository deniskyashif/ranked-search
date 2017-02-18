namespace RankedSearch.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class InvertedIndexTests
    {
        public IList<Document> Documents { get; set; }

        [TestInitialize]
        public void PrepareTestData()
        {
            this.Documents = new[]
            {
                new Document { Id = "1", Title = "1", BagOfWords = new BagOfWords(new [] { "a", "Man", "does", "the", "job" }) },
                new Document { Id = "2", Title = "2", BagOfWords = new BagOfWords(new [] { "whatever", "you", "do" }) },
                new Document { Id = "3", Title = "3", BagOfWords = new BagOfWords(new [] { "see", "go", "do" }) },
                new Document { Id = "4", Title = "4", BagOfWords = new BagOfWords(new [] { "do", "me", "a", "favor" }) },
                new Document { Id = "5", Title = "5", BagOfWords = new BagOfWords(new [] { "you", "turn", "on", "the", "lights" }) },
                new Document { Id = "6", Title = "6", BagOfWords = new BagOfWords(new [] { "you", "give", "whatever", "you", "can" }) }
            };
        }

        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfDocuments_ShouldCreateANewInstance()
        {
            var invertedIndex = new InvertedIndex(new[] { new Document() });
            Assert.IsInstanceOfType(invertedIndex, typeof(InvertedIndex));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingAnInstance_WithProvidingNullArgument_ShouldThrowArgumentException()
        {
            var invertedIndex = new InvertedIndex(null);
        }

        [TestMethod]
        public void GettingDocuments_ContainingATerm_ShouldReturnCorrectResult1()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentsContainingTerm("do").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(new[] { "2", "3", "4" }, actual);
        }

        [TestMethod]
        public void GettingDocuments_ContainingATerm_ShouldReturnCorrectResult2()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentsContainingTerm("the").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(new[] { "1", "5" }, actual);
        }

        [TestMethod]
        public void GettingDocuments_ContainingATerm_ShouldReturnCorrectResult3()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentsContainingTerm("favor").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(new[] { "4" }, actual);
        }

        [TestMethod]
        public void GettingDocuments_ContainingATerm_ShouldReturnCorrectResult4()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentsContainingTerm("a").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(new[] { "1", "4" }, actual);
        }

        [TestMethod]
        public void GettingDocuments_ContainingATerm_ShouldReturnCorrectResult5()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentsContainingTerm("you").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(new[] { "2", "5", "6" }, actual);
        }

        [TestMethod]
        public void GettingDocuments_ProvidingUnseenTerm_ShouldReturnEmptyCollection()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentsContainingTerm("bear").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(new string[] { }, actual);
        }

        [TestMethod]
        public void GettingDocFrequency_ProvidingATerm_ShouldReturnCorrectResult1()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentFrequency("Man");

            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void GettingDocFrequency_ProvidingATerm_ShouldReturnCorrectResult2()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentFrequency("on");

            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void GettingDocFrequency_ProvidingATerm_ShouldReturnCorrectResult3()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentFrequency("do");

            Assert.AreEqual(3, actual);
        }

        [TestMethod]
        public void GettingDocFrequency_ProvidingATerm_ShouldReturnCorrectResult4()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentFrequency("the");

            Assert.AreEqual(2, actual);
        }

        [TestMethod]
        public void GettingDocFrequency_ProvidingATerm_ShouldReturnCorrectResult5()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentFrequency("whatever");

            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void GettingDocFrequency_ProvidingUnseenTerm_ShouldReturnZero()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var actual = invertedIndex.GetDocumentFrequency("navigator");

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void GettingTheInverseDocFrequency_ProvidingATerm_ShouldReturnCorrectResult1()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var expected = Math.Round(Math.Log(6 / 2), 2);
            var actual = Math.Round(invertedIndex.GetInverseDocumentFrequency("the"), 2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GettingTheInverseDocFrequency_ProvidingATerm_ShouldReturnCorrectResult2()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var expected = Math.Round(Math.Log(6 / 3));
            var actual = Math.Round(invertedIndex.GetInverseDocumentFrequency("do"));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GettingTheInverseDocFrequency_ProvidingATerm_ShouldReturnCorrectResult3()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var expected = Math.Round(Math.Log(6 / 1));
            var actual = invertedIndex.GetInverseDocumentFrequency("favor");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GettingTheInverseDocFrequency_ProvidingATerm_ShouldReturnCorrectResult4()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var expected = Math.Round(Math.Log(6 / 2));
            var actual = invertedIndex.GetInverseDocumentFrequency("a");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GettingTheInverseDocFrequency_ProvidingATerm_ShouldReturnCorrectResult5()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var expected = Math.Round(Math.Log(6 / 3));
            var actual = invertedIndex.GetInverseDocumentFrequency("you");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GettingTheInverseDocFrequency_ProvidingUnseenTerm_ShouldReturnZero()
        {
            var invertedIndex = new InvertedIndex(this.Documents);
            var expected = 0;
            var actual = invertedIndex.GetInverseDocumentFrequency("tree");

            Assert.AreEqual(expected, actual);
        }
    }
}
