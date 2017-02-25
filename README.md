# Ranked Search

[![Build status](https://ci.appveyor.com/api/projects/status/7b7hm5wbdjffxb9d?svg=true)](https://ci.appveyor.com/project/deniskyashif/ranked-search)

Search system for the [Reuters 21578 Corpus](https://archive.ics.uci.edu/ml/datasets/Reuters-21578+Text+Categorization+Collection)  

It uses an [**_inverted index_**](http://nlp.stanford.edu/IR-book/html/htmledition/a-first-take-at-building-an-inverted-index-1.html) to store the documents and each document represented as a _bag of words_ model where each word is reduced to its stem before inferring the probability distribution.  
The relevance score of a document is computed by summing the query terms' [**_tf-idf_**](http://www.tfidf.com/) weights:  

### Clone
```
git clone --recursive https://github.com/deniskyashif/ranked-search.git
```

### Dependencies
* .NET Framework 4.5.2
