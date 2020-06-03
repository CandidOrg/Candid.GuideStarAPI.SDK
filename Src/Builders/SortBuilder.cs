namespace Candid.GuideStarAPI
{
  class SortBuilder : ISortBuilder
  {
    protected Sort _sort;

    private SortBuilder() => _sort = new Sort();
    internal static SortBuilder Create() => new SortBuilder();

    public ISortBuilder SortBy(string sort)
    {
      _sort.sort_by = sort;
      return this;
    }

    public ISortBuilder SortByAscending()
    {
      _sort.ascending = true;
      return this;
    }

    public Sort Build() => _sort;
  }
}
