namespace Candid.GuideStarAPI
{
  public interface ISortBuilder
  {
    ISortBuilder SortBy(string sort);
    ISortBuilder SortByAscending();
  }
}
