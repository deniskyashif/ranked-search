namespace Client.Wpf
{
    using RankedSearch;
    using RankedSearch.Poco;
    using RankedSearch.Stemmers;
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Windows;

    public class SearchViewModel: DependencyObject
    {
        private const int SearchResultsLimit = 50;
        private readonly string corpusPath = ConfigurationManager.AppSettings["corpusPath"];
        private readonly SearchEngine searchEngine;
        
        // Using a DependencyProperty as the backing store for SearchResults.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchResultsProperty =
            DependencyProperty.Register("SearchResults", typeof(ObservableCollection<SearchResult>), typeof(SearchViewModel), new PropertyMetadata(null));

        public SearchViewModel()
        {
            this.searchEngine = new SearchEngine(new PorterStemmer());
            this.searchEngine.LoadDocuments(corpusPath);
            this.DocumentCount = this.searchEngine.DocumentCount;
        }

        public int DocumentCount { get; private set; }

        private void UpdateSearchResults()
        {
            try
            {
                this.SearchResults = new ObservableCollection<SearchResult>(
                    this.searchEngine.Search(this.SearchQuery, SearchResultsLimit));
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<SearchResult> SearchResults
        {
            get { return (ObservableCollection<SearchResult>)GetValue(SearchResultsProperty); }
            set { SetValue(SearchResultsProperty, value); }
        }

        public string SearchQuery
        {
            get { return (string)GetValue(SearchQueryProperty); }
            set { SetValue(SearchQueryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchQuery.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchQueryProperty = DependencyProperty.Register("SearchQuery", typeof(string), typeof(SearchViewModel), new PropertyMetadata(string.Empty));

        public void Search()
        {
            this.UpdateSearchResults();
        }

    }
}
