namespace RankedSearch
{
    using Extensions;
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

        public IEnumerable<Document> GetDocumentsContainingTerms(IEnumerable<string> terms)
        {
            var result = new HashSet<Document>();

            terms.ForEach(term =>
            {
                var docsForTerms = this.GetDocumentsContainingTerm(term);
                docsForTerms.ForEach(doc => result.Add(doc));
            });

            return result;
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
            var terms = new HashSet<string>();
            documents.ForEach(doc =>
                doc.BagOfWords.DistinctTerms.ForEach(t => terms.Add(t)));

            var result = new Dictionary<string, IEnumerable<Document>>();
            terms.ForEach(term => result.Add(term, documents.Where(d => d.BagOfWords.GetTermCount(term) > 0)));

            return result;
        }
    }
}
