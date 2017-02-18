namespace RankedSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class InvertedIndex
    {
        // term -> df, tfds
        public InvertedIndex(IEnumerable<Document> documents)
        {
            if (documents == null)
                throw new ArgumentNullException("Documents collection should not be null.");
        }

        public IEnumerable<Document> GetDocumentsContainingTerm(string term)
        {
            throw new NotImplementedException();
        }

        public double GetDocumentFrequency(string term)
        {
            throw new NotImplementedException();
        }

        public double GetInverseDocumentFrequency(string term)
        {
            throw new NotImplementedException();
        }
    }
}
