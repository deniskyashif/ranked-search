namespace Client.Wpf
{
    using Iveonik.Stemmers;
    using RankedSearch;
    using RankedSearch.Poco;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class SearchViewModel: DependencyObject
    {
        private readonly string corpusPath = ConfigurationManager.AppSettings["corpusPath"];
        private readonly SearchEngine searchEngine;

        public SearchViewModel()
        {
            this.searchEngine = new SearchEngine(new EnglishStemmer());
            this.searchEngine.LoadDocuments(corpusPath);
        }

        private void UpdateSearchResults()
        {
            this.SearchResults = new ObservableCollection<SearchResult>(this.searchEngine.Search(this.SearchQuery));
        }

        public ObservableCollection<SearchResult> SearchResults
        {
            get { return (ObservableCollection<SearchResult>)GetValue(SearchResultsProperty); }
            set { SetValue(SearchResultsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchResults.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchResultsProperty =
            DependencyProperty.Register("SearchResults", typeof(ObservableCollection<SearchResult>), typeof(SearchViewModel), new PropertyMetadata(null));
        
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
