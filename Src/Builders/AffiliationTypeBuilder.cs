using Candid.GuideStarAPI.Src.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI.Src.Builders
{
  class AffiliationTypeBuilder : IAffiliationTypeBuilder
  {
    protected Affiliation_Type _affiliationType;
    private AffiliationTypeBuilder() => _affiliationType = new Affiliation_Type();

    internal static AffiliationTypeBuilder Create() => new AffiliationTypeBuilder(); 

    public IAffiliationTypeBuilder OnlyHeadquarters()
    {
      _affiliationType.headquarter = true;
      return this;
    }

    public IAffiliationTypeBuilder OnlyIndependent()
    {
      _affiliationType.independent = true;
      return this;
    }

    public IAffiliationTypeBuilder OnlyParents()
    {
      _affiliationType.parent = true;
      return this;
    }

    public IAffiliationTypeBuilder OnlySubordinate()
    {
      _affiliationType.subordinate = true;
      return this;
    }

    internal Affiliation_Type Build() => _affiliationType;
  }
}
