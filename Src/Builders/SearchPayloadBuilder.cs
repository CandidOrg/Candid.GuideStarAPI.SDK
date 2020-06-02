using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI
{
  public class SearchPayloadBuilder : ISearchPayloadBuilder
  {
    protected SearchPayload _request;
    private IFilterBuilder _filterBuilder;

    private SearchPayloadBuilder() => _request = new SearchPayload();

    public ISearchPayloadBuilder Create() => new SearchPayloadBuilder();

    public ISearchPayloadBuilder WithSearchTerms(string searchTerms)
    {      
      _request.search_terms = searchTerms;
      return this;
    }

    public ISearchPayloadBuilder From(int from)
    {
      if (from > 0)
      {
        _request.from = from;
        return this;
      }
      throw new Exception("From must be greater than 0");
    }

    public ISearchPayloadBuilder Size(int to)
    {
      if (to > 0)
      {
        _request.size = to;
        return this;
      }
      throw new Exception("To must be greater than 0");
    }

    public ISearchPayloadBuilder Sort()
    {
      // whenever this gets implemented
      // _request.sort = new 
      return this;
    }

    public IFilterBuilder Filters()
    {
      _filterBuilder = FilterBuilder.Create();
      return _filterBuilder;
    }

    public SearchPayload Build() => _request;
  }
}
