using System;

namespace Candid.GuideStarAPI
{
  public class SearchPayloadBuilder
  {
    protected SearchPayload _request;

    private SearchPayloadBuilder() => _request = new SearchPayload();

    public static SearchPayloadBuilder Create() => new SearchPayloadBuilder();

    public SearchPayloadBuilder WithSearchTerms(string searchTerms)
    {
      _request.search_terms = searchTerms;
      return this;
    }

    public SearchPayloadBuilder From(int from)
    {
      if (from >= 0)
      {
        _request.from = from;
        return this;
      }
      throw new ArgumentOutOfRangeException("From must be greater than 0");
    }

    public SearchPayloadBuilder Size(int to)
    {
      if (to >= 0)
      {
        _request.size = to;
        return this;
      }
      throw new ArgumentOutOfRangeException("To must be greater than 0");
    }

    public SearchPayloadBuilder Sort(Action<SortBuilder> action)
    {
      var _sortBuilder = SortBuilder.Create();
      action(_sortBuilder);
      _request.sort = _sortBuilder.Build();
      return this;
    }

    public SearchPayloadBuilder Filters(Action<FilterBuilder> action)
    {
      var _filterBuilder = FilterBuilder.Create();
      action(_filterBuilder);
      _request.filters = _filterBuilder.Build();
      return this;
    }

    public SearchPayload Build() => _request;
  }
}
