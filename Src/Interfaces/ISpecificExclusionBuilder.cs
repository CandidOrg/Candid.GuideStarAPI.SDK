using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI.Src.Interfaces
{
  interface ISpecificExclusionBuilder
  {
    ISpecificExclusionBuilder ExcludeRevokedOrganizations();
    ISpecificExclusionBuilder ExcludeDefunctOrMergedOrganizations();
  }
}
