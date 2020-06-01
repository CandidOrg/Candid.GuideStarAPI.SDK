using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI
{
  public class SearchRequestBuilder : ISearchRequestBuilder
  {
    protected SearchRequest _request;

    private SearchRequestBuilder() => _request = new SearchRequest();

    public static SearchRequestBuilder Create() => new SearchRequestBuilder();

    public SearchRequestBuilder WithSearchTerms(string searchTerms)
    {      
      _request.search_terms = searchTerms;
      return this;
    }

    ISearchRequestBuilder From(int from)
    {
      if (from > 0)
      {
        _request.from = from;
        return this;
      }
      throw new Exception("From must be greater than 0");
    }

    ISearchRequestBuilder To(int to)
    {
      if (to > 0)
      {
        _request.to = to;
        return this;
      }
      throw new Exception("To must be greater than 0");
    }

    ISearchRequestBuilder Sort()
    {
      // whenever this gets implemented
      // _request.sort = new 
    }

    ISearchRequestBuilder Filters()
    {
      // whenever this gets implemented
      //var builder = new FilterBuilder();
    }

    public SearchRequest Build() => _request;
  }
}
