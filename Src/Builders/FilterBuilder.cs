using System;

namespace Candid.GuideStarAPI
{
  internal class FilterBuilder : IFilterBuilder
  {
    protected Filters _filter;

    private FilterBuilder() => _filter = new Filters();
    internal static FilterBuilder Create() => new FilterBuilder();

    public IFilterBuilder Geography(Action<IGeographyBuilder> action)
    {
      var _geoBuilder = GeographyBuilder.Create();
      action(_geoBuilder);
      _filter.geography = _geoBuilder.Build();
      return this;
    }

    public IFilterBuilder Organization(Action<IOrganizationBuilder> action)
    {
      var _orgBuilder = OrganizationBuilder.Create();
      action(_orgBuilder);
      _filter.organization = _orgBuilder.Build();
      return this;
    }

    public IFilterBuilder Financials(Action<IFinancialsBuilder> action)
    {
      var _finBuilder = FinancialsBuilder.Create();
      action(_finBuilder);
      _filter.financials = _finBuilder.Build();
      return this;
    }

    internal Filters Build() => _filter;
  }
}
