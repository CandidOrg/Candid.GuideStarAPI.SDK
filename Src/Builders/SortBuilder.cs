namespace Candid.GuideStarAPI
{
  public class SortBuilder
  {
    protected Sort _sort;

    private SortBuilder() => _sort = new Sort();
    internal static SortBuilder Create() => new SortBuilder();

    public SortBuilder SortBy(string sort)
    {
      _sort.sort_by = sort;
      return this;
    }

    public SortBuilder SortByAscending()
    {
      _sort.ascending = true;
      return this;
    }

    public Sort Build() => _sort;
  }
}
