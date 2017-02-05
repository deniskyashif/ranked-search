namespace RankedSearch.Poco
{
    public class SearchResult
    {
        public SearchResult(Document document, double score)
        {
            this.Document = document;
            this.RelevanceScore = score;
        }

        public Document Document { get; private set; }

        public double RelevanceScore { get; private set; }
    }
}
