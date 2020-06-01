public interface IFilterBuilder
{
  IFilterBuilder WithSearchTerms(string searchTerms);
  IFilterBuilder OnlyParentOrgs();
}