using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RankedSearch.Tests
{
    [TestClass]
    public class InvertedIndexTests
    {
        [TestMethod]
        public void CreatingAnInstance_WithProvidingEnumerableOfDocuments_ShouldCreateANewInstance()
        {
            var index = new InvertedIndex(new[] { new Document() });
            Assert.IsInstanceOfType(index, typeof(InvertedIndex));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatingAnInstance_WithProvidingNullArgument_ShouldThrowArgumentException()
        {
            var index = new InvertedIndex(null);
        }
    }
}
