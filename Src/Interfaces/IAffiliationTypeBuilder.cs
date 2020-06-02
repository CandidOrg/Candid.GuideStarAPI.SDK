using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI.Src.Interfaces
{
  interface IAffiliationTypeBuilder
  {
    IAffiliationTypeBuilder OnlyParents();
    IAffiliationTypeBuilder OnlySubordinate();
    IAffiliationTypeBuilder OnlyIndependent();
    IAffiliationTypeBuilder OnlyHeadquarters();
  }
}
