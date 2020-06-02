using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI
{
  public class SearchPayloadBuilder : ISearchRequestBuilder
  {
    protected SearchPayload _request;

    private SearchPayloadBuilder() => _request = new SearchPayload();

    public ISearchRequestBuilder Create() => new SearchPayloadBuilder();

    public ISearchRequestBuilder WithSearchTerms(string searchTerms)
    {      
      _request.search_terms = searchTerms;
      return this;
    }

    public ISearchRequestBuilder From(int from)
    {
      if (from > 0)
      {
        _request.from = from;
        return this;
      }
      throw new Exception("From must be greater than 0");
    }

    public ISearchRequestBuilder Size(int to)
    {
      if (to > 0)
      {
        _request.size = to;
        return this;
      }
      throw new Exception("To must be greater than 0");
    }

    public ISearchRequestBuilder Sort()
    {
      // whenever this gets implemented
      // _request.sort = new 
      return this;
    }

    public ISearchRequestBuilder Filters()
    {
      // whenever this gets implemented
      //var builder = new FilterBuilder();
      return this;
    }

    public SearchPayload Build() => _request;
  }
}
