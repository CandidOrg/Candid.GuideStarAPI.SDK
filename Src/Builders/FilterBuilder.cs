using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI
{
  class FilterBuilder : IFilterBuilder
  {
    protected Filters _filter;
    protected GeographyBuilder _geoBuilder;
    //protected IOrganizationBuilder _orgBuilder;
    //protected IFinancialsBuilder _finBuilder;

    private FilterBuilder() => _filter = new Filters();
    internal static FilterBuilder Create() => new FilterBuilder();

    public IGeographyBuilder Geography()
    {
      _geoBuilder = GeographyBuilder.Create();
      return _geoBuilder;
    }

    internal Filters Build()
    {
      _filter.geography = _geoBuilder.Build();
      //_filter.organization = _orgBuilder.Build();
      //_filter.geography = _geoBuilder.Build();
      return _filter;
    }
  }
}
