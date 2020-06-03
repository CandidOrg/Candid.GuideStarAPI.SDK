namespace Candid.GuideStarAPI
{
  public interface ISpecificExclusionBuilder
  {
    ISpecificExclusionBuilder ExcludeRevokedOrganizations();
    ISpecificExclusionBuilder ExcludeDefunctOrMergedOrganizations();
  }
}
