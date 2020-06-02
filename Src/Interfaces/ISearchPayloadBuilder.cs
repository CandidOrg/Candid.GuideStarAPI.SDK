namespace Candid.GuideStarAPI
{
  public interface ISearchPayloadBuilder
  {
    ISearchPayloadBuilder Create();
    ISearchPayloadBuilder WithSearchTerms(string searchTerms);
    ISearchPayloadBuilder From(int from);
    ISearchPayloadBuilder Size(int size);
    ISearchPayloadBuilder Sort();
    IFilterBuilder Filters();
  }
}