public interface ISearchRequestBuilder
{
  ISearchRequestBuilder Create();
  ISearchRequestBuilder WithSearchTerms(string searchTerms);
  ISearchRequestBuilder From(int from);
  ISearchRequestBuilder Size (int size);
  ISearchRequestBuilder Sort ();
  ISearchRequestBuilder Filters();
}