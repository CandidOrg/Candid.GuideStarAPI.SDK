public interface ISearchRequestBuilder
{
  ISearchRequestBuilder Create();
  ISearchRequestBuilder WithSearchTerms(string searchTerms);
  ISearchRequestBuilder From(int from);
  ISearchRequestBuilder To (int to);
  ISearchRequestBuilder Sort ();
  ISearchRequestBuilder Filters();
}