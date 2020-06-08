namespace Candid.GuideStarAPI
{
  public class SpecificExclusionBuilder
  {
    protected Specific_Exclusions _exclusions;

    private SpecificExclusionBuilder() => _exclusions = new Specific_Exclusions();

    internal static SpecificExclusionBuilder Create() => new SpecificExclusionBuilder();

    public SpecificExclusionBuilder ExcludeDefunctOrMergedOrganizations()
    {
      _exclusions.exclude_defunct_or_merged_organizations = true;
      return this;
    }

    public SpecificExclusionBuilder ExcludeRevokedOrganizations()
    {
      _exclusions.exclude_revoked_organizations = true;
      return this;
    }

    internal Specific_Exclusions Build()
    {
      return _exclusions;
    }
  }
}
