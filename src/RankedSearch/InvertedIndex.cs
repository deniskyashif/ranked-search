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

        public IEnumerable<Document> GetDocumentsContainingTerms(IEnumerable<string> terms)
        {
            var result = new HashSet<Document>();

            foreach (var term in terms)
            {
                var docsForTerms = this.GetDocumentsContainingTerm(term);
                foreach (var doc in docsForTerms)
                {
                    result.Add(doc);
				}
            }

            return result;
        }

        public IEnumerable<Document> GetDocumentsContainingTerm(string term)
        {
            if (this.invertedIndex.ContainsKey(term))
                return this.invertedIndex[term];

			return Array.Empty<Document>();
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

            foreach (var doc in documents)
            {
                foreach (var term in doc.BagOfWords.DistinctTerms)
                {
                    terms.Add(term);
                }
			}

            var result = new Dictionary<string, IEnumerable<Document>>();

            foreach (var term in terms)
            {
                result.Add(term, documents.Where(d => d.BagOfWords.GetTermCount(term) > 0));
			}

            return result;
        }
    }
}
