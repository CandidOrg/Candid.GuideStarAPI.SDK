using System;

namespace Candid.GuideStarAPI
{
  public class SearchPayloadBuilder : ISearchPayloadBuilder
  {
    protected SearchPayload _request;
    
    private SearchPayloadBuilder() => _request = new SearchPayload();

    public static ISearchPayloadBuilder Create() => new SearchPayloadBuilder();

    public ISearchPayloadBuilder WithSearchTerms(string searchTerms)
    {
      _request.search_terms = searchTerms;
      return this;
    }

    public ISearchPayloadBuilder From(int from)
    {
      if (from >= 0)
      {
        _request.from = from;
        return this;
      }
      throw new Exception("From must be greater than 0");
    }

    public ISearchPayloadBuilder Size(int to)
    {
      if (to >= 0)
      {
        _request.size = to;
        return this;
      }
      throw new Exception("To must be greater than 0");
    }

    public ISearchPayloadBuilder Sort(Action<ISortBuilder> action)
    {
      var _sortBuilder = SortBuilder.Create();
      action(_sortBuilder);
      _request.sort = _sortBuilder.Build();
      return this;
    }

    public ISearchPayloadBuilder Filters(Action<IFilterBuilder> action)
    {
      var _filterBuilder = FilterBuilder.Create();
      action(_filterBuilder);
      _request.filters = _filterBuilder.Build();
      return this;
    }

    public SearchPayload Build() => _request;
  }
}
