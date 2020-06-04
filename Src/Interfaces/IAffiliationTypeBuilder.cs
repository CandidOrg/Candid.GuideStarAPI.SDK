namespace Candid.GuideStarAPI
{
  public interface IAffiliationTypeBuilder
  {
    IAffiliationTypeBuilder OnlyParents();
    IAffiliationTypeBuilder OnlySubordinate();
    IAffiliationTypeBuilder OnlyIndependent();
    IAffiliationTypeBuilder OnlyHeadquarters();
  }
}
