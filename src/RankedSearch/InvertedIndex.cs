namespace RankedSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class InvertedIndex
    {
        private readonly int documentsCount;
        private readonly IDictionary<string, IEnumerable<Document>> invertedIndex;
        
        public InvertedIndex(IEnumerable<Document> documents)
        {
            if (documents == null)
                throw new ArgumentNullException("Documents collection should not be null.");

            this.documentsCount = documents.Count();
            this.invertedIndex = this.ConstructInvertedIndex(documents);
        }

        public IEnumerable<Document> GetDocumentsContainingTerm(string term)
        {
            if (this.invertedIndex.ContainsKey(term))
                return this.invertedIndex[term];

            return new Document[0];
        }

        public double GetDocumentFrequency(string term)
        {
            return this.GetDocumentsContainingTerm(term).Count();
        }

        public double GetInverseDocumentFrequency(string term)
        {
            var docFrequency = this.GetDocumentFrequency(term);

            if(docFrequency > 0)
                return Math.Log(documentsCount / docFrequency);

            return 0;
        }

        private IDictionary<string, IEnumerable<Document>> ConstructInvertedIndex(IEnumerable<Document> documents)
        {
            throw new NotImplementedException();
        }
    }
}
