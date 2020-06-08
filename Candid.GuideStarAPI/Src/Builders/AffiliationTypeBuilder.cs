namespace Candid.GuideStarAPI
{
  public class AffiliationTypeBuilder
  {
    protected Affiliation_Type _affiliationType;
    private AffiliationTypeBuilder() => _affiliationType = new Affiliation_Type();

    internal static AffiliationTypeBuilder Create() => new AffiliationTypeBuilder();

    public AffiliationTypeBuilder OnlyHeadquarters()
    {
      _affiliationType.headquarter = true;
      return this;
    }

    public AffiliationTypeBuilder OnlyIndependent()
    {
      _affiliationType.independent = true;
      return this;
    }

    public AffiliationTypeBuilder OnlyParents()
    {
      _affiliationType.parent = true;
      return this;
    }

    public AffiliationTypeBuilder OnlySubordinate()
    {
      _affiliationType.subordinate = true;
      return this;
    }

    internal Affiliation_Type Build() => _affiliationType;
  }
}
