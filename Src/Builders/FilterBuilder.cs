using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI
{
  class FilterBuilder : IFilterBuilder
  {
    protected IGeographyBuilder _geoBuilder;
    //protected IOrganizationBuilder _orgBuilder;
    //protected IFinancialsBuilder _finBuilder;

    public IGeographyBuilder Geology()
    {
      _geoBuilder = GeographyBuilder.Create();
      return _geoBuilder;
    }
  }
}
